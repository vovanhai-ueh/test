# -*- coding: utf-8 -*-
"""Đọc kết quả biên dịch + tệp TRX của một buổi, lập báo cáo tiếng Việt.

Dùng:  python3 bao_cao.py --buoi Buoi05 --thu-muc KetQua
Tạo:   KetQua/<buoi>.json, KetQua/<buoi>.md; ghi thêm vào GITHUB_STEP_SUMMARY.
Mã thoát 1 nếu biên dịch lỗi hoặc có bài làm sai (bài "Chưa làm" không tính là lỗi,
trừ khi biến môi trường CHUA_LAM_LA_LOI=true).
"""
import argparse, glob, json, os, re, sys
import xml.etree.ElementTree as ET

NS = {"t": "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"}
THU_MUC_SCRIPT = os.path.dirname(os.path.abspath(__file__))
NHAN = {"dat": "✅ Đạt", "lam-do": "🟡 Làm dở", "chua-lam": "⬜ Chưa làm", "loi": "❌ Có lỗi"}


def doc_hop_dong(buoi):
    try:
        with open(os.path.join(THU_MUC_SCRIPT, "hop-dong.json"), encoding="utf-8") as f:
            return {b["bai"]: b for b in json.load(f).get(buoi, [])}
    except OSError:
        return {}


def loi_bien_dich(log_path):
    if not os.path.exists(log_path):
        return []
    goc = os.environ.get("GITHUB_WORKSPACE", os.getcwd()).rstrip("/") + "/"
    mau = re.compile(r"^\s*(?P<tep>[^\s(][^(]*?)\((?P<dong>\d+),(?P<cot>\d+)\): error (?P<ma>[A-Z]+\d+): (?P<tb>.*?)(?: \[[^\]]*\])?$")
    ds, thay = [], set()
    with open(log_path, encoding="utf-8", errors="replace") as f:
        for dong in f:
            m = mau.match(dong.rstrip())
            if not m:
                continue
            tep = m["tep"].replace("\\", "/").replace(goc, "")
            k = (tep, m["dong"], m["ma"])
            if k in thay:
                continue
            thay.add(k)
            ds.append({"tep": tep, "dong": int(m["dong"]), "cot": int(m["cot"]), "ma": m["ma"], "thongBao": m["tb"]})
    return ds


def doc_trx(duong_dan):
    goc = ET.parse(duong_dan).getroot()
    lop = {}
    for ut in goc.iterfind("t:TestDefinitions/t:UnitTest", NS):
        tm = ut.find("t:TestMethod", NS)
        lop[ut.get("id")] = tm.get("className") if tm is not None else ""
    kq = []
    for r in goc.iterfind("t:Results/t:UnitTestResult", NS):
        tb = r.find("t:Output/t:ErrorInfo/t:Message", NS)
        ten = (r.get("testName") or "").replace("\r", "").replace("\n", "↵")
        kq.append({"lop": lop.get(r.get("testId"), ""), "ten": ten,
                   "ketQua": r.get("outcome"), "thongBao": (tb.text or "").strip() if tb is not None else ""})
    return kq


def phan_loai(thong_bao):
    m = re.search(r"\[([A-ZÀ-Ỹ ]+)\]", thong_bao or "")
    return m.group(1) if m else "LỖI"


def lap_bao_cao(buoi, thu_muc):
    hop_dong = doc_hop_dong(buoi)
    loi_bd = loi_bien_dich(os.path.join(thu_muc, "build.log"))
    trx = sorted(glob.glob(os.path.join(thu_muc, "**", f"{buoi}*.trx"), recursive=True))
    bien_dich_ok = os.path.exists(os.path.join(thu_muc, "build.ok")) or (bool(trx) and not loi_bd)
    chua_lam_la_loi = os.environ.get("CHUA_LAM_LA_LOI", "false").lower() == "true"

    bai = {}
    for ma, hd in hop_dong.items():
        bai[ma] = {"tieuDe": hd.get("tieuDe", ""), "tong": 0, "dat": 0, "chuaLam": 0, "loi": 0, "test": []}
    for t in (doc_trx(trx[0]) if trx else []):
        m = re.search(r"\.([AB]\d+)_", t["lop"] + "_")
        ma = m.group(1) if m else (t["ten"].split(" ")[0] if t["ten"] else "?")
        b = bai.setdefault(ma, {"tieuDe": "", "tong": 0, "dat": 0, "chuaLam": 0, "loi": 0, "test": []})
        b["tong"] += 1
        if t["ketQua"] == "Passed":
            b["dat"] += 1
        elif "[CHƯA LÀM]" in t["thongBao"]:
            b["chuaLam"] += 1
            b["test"].append(t)
        else:
            b["loi"] += 1
            b["test"].append(t)

    for b in bai.values():
        if not bien_dich_ok:
            b["trangThai"] = "loi"
        elif b["tong"] == 0:
            b["trangThai"] = "chua-lam"
        elif b["dat"] == b["tong"]:
            b["trangThai"] = "dat"
        elif b["loi"] > 0:
            b["trangThai"] = "loi"
        elif b["dat"] == 0:
            b["trangThai"] = "chua-lam"
        else:
            b["trangThai"] = "lam-do"
        b["diem"] = round(b["dat"] / b["tong"], 4) if b["tong"] and bien_dich_ok else 0.0

    so_bai = len(bai) or 1
    diem = round(10 * sum(b["diem"] for b in bai.values()) / so_bai, 2)
    co_loi = (not bien_dich_ok) or any(b["loi"] > 0 for b in bai.values()) or \
             (chua_lam_la_loi and any(b["chuaLam"] > 0 for b in bai.values()))
    return {"buoi": buoi, "bienDichOk": bien_dich_ok, "loiBienDich": loi_bd, "bai": dict(sorted(bai.items(), key=lambda kv: (kv[0][0], int(kv[0][1:])))),
            "tongTest": sum(b["tong"] for b in bai.values()), "testDat": sum(b["dat"] for b in bai.values()),
            "baiDat": sum(1 for b in bai.values() if b["trangThai"] == "dat"), "soBai": len(bai),
            "diem": diem, "coLoi": co_loi, "coTrx": bool(trx)}


def cat(s, n=1200):
    s = s or ""
    return s if len(s) <= n else s[:n] + "\n…(đã cắt bớt)"


def markdown(bc, chi_tiet=True):
    ten = f"Buổi {int(bc['buoi'][4:])}"
    dong = []
    if not bc["bienDichOk"]:
        dong.append(f"### ❌ {ten}: biên dịch thất bại\n")
        dong.append("Bộ kiểm thử không chạy được vì code chưa biên dịch. Sửa các lỗi dưới đây (Visual Studio gạch đỏ đúng các dòng này), "
                    "chạy lại `dotnet build` trên máy cho đến khi hết lỗi rồi push lại.\n")
        dong.append("| Tệp | Dòng | Mã lỗi | Thông báo |\n| --- | ---: | --- | --- |")
        for e in bc["loiBienDich"][:30]:
            tb = e["thongBao"].replace("|", "/")
            dong.append(f"| `{e['tep']}` | {e['dong']} | {e['ma']} | {tb} |")
        if not bc["loiBienDich"]:
            dong.append("| (xem tệp build.log trong artifact) | | | |")
        return "\n".join(dong) + "\n"

    bieu_tuong = "❌" if bc["coLoi"] else "✅"
    dong.append(f"### {bieu_tuong} {ten}: {bc['baiDat']}/{bc['soBai']} bài đạt · {bc['testDat']}/{bc['tongTest']} test · điểm {bc['diem']:.2f}/10\n")
    dong.append("| Bài | Nội dung | Kết quả | Test đạt |\n| --- | --- | --- | ---: |")
    for ma, b in bc["bai"].items():
        dong.append(f"| {ma} | {b['tieuDe']} | {NHAN[b['trangThai']]} | {b['dat']}/{b['tong']} |")
    dong.append("")
    if chi_tiet:
        for ma, b in bc["bai"].items():
            loi = [t for t in b["test"] if "[CHƯA LÀM]" not in t["thongBao"]]
            if not loi:
                continue
            dong.append(f"<details><summary><b>{ma}</b> – {len(loi)} test chưa đạt</summary>\n")
            for t in loi[:8]:
                dong.append(f"**{t['ten']}**\n\n```text\n{cat(t['thongBao'])}\n```\n")
            if len(loi) > 8:
                dong.append(f"_…và {len(loi) - 8} test khác (xem tệp HTML trong artifact)._\n")
            dong.append("</details>\n")
    return "\n".join(dong) + "\n"


def chu_thich_github(bc):
    """In annotation để GitHub hiện lỗi ngay trên trang Pull Request (tối đa 10)."""
    n = 0
    for ma, b in bc["bai"].items():
        for t in b["test"]:
            if "[CHƯA LÀM]" in t["thongBao"] or n >= 10:
                continue
            tep = f"{bc['buoi']}/{bc['buoi']}_{ma}/Program.cs"
            tb = t["thongBao"].split("\n")[0].replace("%", "%25").replace("\r", "").replace("\n", "%0A")
            print(f"::error file={tep},title={bc['buoi']} {ma} – {phan_loai(t['thongBao'])}::{tb}")
            n += 1


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--buoi", required=True)
    ap.add_argument("--thu-muc", default="KetQua")
    a = ap.parse_args()
    bc = lap_bao_cao(a.buoi, a.thu_muc)
    md = markdown(bc)
    os.makedirs(a.thu_muc, exist_ok=True)
    with open(os.path.join(a.thu_muc, f"{a.buoi}.json"), "w", encoding="utf-8") as f:
        json.dump(bc, f, ensure_ascii=False, indent=1)
    with open(os.path.join(a.thu_muc, f"{a.buoi}.md"), "w", encoding="utf-8") as f:
        f.write(md)
    tom_tat = os.environ.get("GITHUB_STEP_SUMMARY")
    if tom_tat:
        with open(tom_tat, "a", encoding="utf-8") as f:
            f.write(md)
    print(md)
    chu_thich_github(bc)
    sys.exit(1 if bc["coLoi"] else 0)


if __name__ == "__main__":
    main()
