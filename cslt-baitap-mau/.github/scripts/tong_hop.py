# -*- coding: utf-8 -*-
"""Gộp báo cáo của các buổi (artifact ket-qua-*) thành một bảng tổng hợp.

Dùng:  python3 tong_hop.py <thư mục artifact>            → ghi tom-tat, binh-luan.md
       python3 tong_hop.py <thư mục artifact> --ket-luan → mã thoát 1 nếu chưa được merge
"""
import glob, json, os, sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from bao_cao import markdown  # noqa: E402


def doc(thu_muc):
    bc = {}
    for p in glob.glob(os.path.join(thu_muc, "**", "Buoi0*.json"), recursive=True):
        with open(p, encoding="utf-8") as f:
            d = json.load(f)
        bc[d["buoi"]] = d
    chuan_bi = {}
    for p in glob.glob(os.path.join(thu_muc, "**", "chuan-bi.json"), recursive=True):
        with open(p, encoding="utf-8") as f:
            chuan_bi = json.load(f)
    return dict(sorted(bc.items())), chuan_bi


def ket_luan(bc, chuan_bi):
    """Trả về (được merge?, lý do)."""
    if chuan_bi.get("viPham"):
        return False, "sửa tệp được bảo vệ"
    thieu = [b for b in chuan_bi.get("buoi", []) if b not in bc]
    if thieu:
        return False, "không có kết quả cho " + ", ".join(thieu) + " (job kiểm thử bị hủy hoặc lỗi hệ thống)"
    loi = [b for b, d in bc.items() if d["coLoi"]]
    if loi:
        return False, "có lỗi ở " + ", ".join(f"buổi {int(b[4:])}" for b in loi)
    return True, ""


def noi_dung(bc, chuan_bi):
    duoc, ly_do = ket_luan(bc, chuan_bi)
    may = os.environ.get("GITHUB_SERVER_URL", "https://github.com")
    repo, run = os.environ.get("GITHUB_REPOSITORY", ""), os.environ.get("GITHUB_RUN_ID", "")
    sha = (os.environ.get("SHA_COMMIT") or os.environ.get("GITHUB_SHA", ""))[:7]
    lien_ket = f"{may}/{repo}/actions/runs/{run}" if repo and run else ""

    d = ["<!-- cslt-kiem-thu -->", "## 🧪 Kết quả kiểm thử tự động", ""]
    if duoc:
        d.append(f"**✅ ĐẠT – được phép merge.** Commit `{sha}`." if bc else "**✅ Không có bài nào thay đổi trong Pull Request này.**")
    else:
        d.append(f"**❌ CHƯA ĐẠT – {ly_do}.** Commit `{sha}`. Sửa theo chi tiết bên dưới rồi push lại; CI sẽ tự chạy lại.")
    d.append("")
    if chuan_bi.get("viPham"):
        d.append("⚠️ Em đã sửa các tệp được bảo vệ (bộ kiểm thử/cấu hình CI). Hãy hoàn tác bằng `git checkout origin/main -- <tệp>`:")
        d += [f"- `{v}`" for v in chuan_bi["viPham"][:20]]
        d.append("")
    if bc:
        d.append("| Buổi | Biên dịch | Bài đạt | Test đạt | Điểm (thang 10) |")
        d.append("| --- | --- | ---: | ---: | ---: |")
        for b, r in bc.items():
            bd = "✅" if r["bienDichOk"] else "❌ lỗi"
            d.append(f"| {int(b[4:])} | {bd} | {r['baiDat']}/{r['soBai']} | {r['testDat']}/{r['tongTest']} | {r['diem']:.2f} |")
        d.append("")
        d.append("Ký hiệu: ✅ đạt · 🟡 làm dở · ⬜ chưa làm (còn `throw new NotImplementedException()`) · ❌ có lỗi.")
        d.append("")
        for b, r in bc.items():
            d.append(markdown(r))
    if lien_ket:
        d.append(f"📄 Báo cáo đầy đủ (tệp HTML, TRX): mục **Artifacts** của [lần chạy này]({lien_ket}).")
    s = "\n".join(d)
    if len(s) > 60000:  # giới hạn bình luận GitHub ~65 nghìn ký tự
        s = s[:60000] + f"\n\n…(đã cắt bớt, xem đầy đủ tại {lien_ket})"
    return s, duoc


def main():
    thu_muc = sys.argv[1] if len(sys.argv) > 1 else "TatCa"
    bc, chuan_bi = doc(thu_muc)
    if "--ket-luan" in sys.argv:
        duoc, ly_do = ket_luan(bc, chuan_bi)
        print("ĐẠT" if duoc else f"CHƯA ĐẠT: {ly_do}")
        sys.exit(0 if duoc else 1)
    s, _ = noi_dung(bc, chuan_bi)
    with open(os.path.join(thu_muc, "binh-luan.md"), "w", encoding="utf-8") as f:
        f.write(s)
    tom_tat = os.environ.get("GITHUB_STEP_SUMMARY")
    if tom_tat:
        with open(tom_tat, "a", encoding="utf-8") as f:
            f.write(s)
    print(s)


if __name__ == "__main__":
    main()
