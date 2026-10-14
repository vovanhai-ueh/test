# -*- coding: utf-8 -*-
"""Chọn các buổi cần kiểm thử và kiểm tra sinh viên có sửa tệp được bảo vệ không.

Pull request: buổi nào có tệp thay đổi trong thư mục BuoiNN/ thì kiểm thử buổi đó.
Chạy tay (workflow_dispatch): theo ô nhập "tat-ca", "5", "3,5", "Buoi07"...
Kết quả: biến `buoi` (mảng JSON) trong GITHUB_OUTPUT và tệp KetQua/chuan-bi.json.
"""
import json, os, re, subprocess

BAO_VE = ("KiemThu/", ".github/", "Directory.Build.props", "BaiTapCSLT.sln")
TAT_CA = [f"Buoi0{i}" for i in range(1, 10)]


def git_diff(goc, dau):
    r = subprocess.run(["git", "diff", "--name-only", f"{goc}...{dau}"], capture_output=True, text=True)
    if r.returncode != 0:  # lịch sử nông: thử so sánh hai đầu
        r = subprocess.run(["git", "diff", "--name-only", goc, dau], capture_output=True, text=True)
    return [l.strip() for l in r.stdout.splitlines() if l.strip()]


def tu_o_nhap(nhap):
    nhap = (nhap or "tat-ca").strip().lower()
    if nhap in ("", "tat-ca", "tatca", "all", "*"):
        return TAT_CA
    ds = []
    for phan in re.split(r"[,\s;]+", nhap):
        m = re.search(r"(\d+)", phan)
        if m and 1 <= int(m.group(1)) <= 9:
            ds.append(f"Buoi0{int(m.group(1))}")
    return sorted(set(ds))


def main():
    su_kien = os.environ.get("SU_KIEN", "")
    vi_pham, thay_doi = [], []
    if su_kien == "pull_request":
        thay_doi = git_diff(os.environ["GOC"], os.environ["DAU"])
        buoi = sorted({m.group(1) for f in thay_doi if (m := re.match(r"(Buoi0[1-9])/", f))})
        vi_pham = [f for f in thay_doi if f.startswith(BAO_VE)]
    else:
        buoi = tu_o_nhap(os.environ.get("NHAP"))

    os.makedirs("KetQua", exist_ok=True)
    with open("KetQua/chuan-bi.json", "w", encoding="utf-8") as f:
        json.dump({"suKien": su_kien, "buoi": buoi, "viPham": vi_pham, "soTepThayDoi": len(thay_doi)}, f, ensure_ascii=False)
    with open(os.environ.get("GITHUB_OUTPUT", os.devnull), "a", encoding="utf-8") as f:
        f.write(f"buoi={json.dumps(buoi)}\n")

    print("Buổi cần kiểm thử:", ", ".join(buoi) or "(không có)")
    for v in vi_pham:
        print(f"::error file={v},title=Sửa tệp được bảo vệ::Không được sửa {v}. Hãy hoàn tác thay đổi này (xem mục Quy định).")


if __name__ == "__main__":
    main()
