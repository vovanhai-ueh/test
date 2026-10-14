using System.IO;
using System.Linq;
using KiemThu.Chung;
using Xunit;

// Buổi 8: mỗi bài kiểm thử chạy trong một thư mục tạm riêng.
namespace KiemThu.Buoi08
{
    public class A1_LopFile
    {
        readonly BaiTap b = new("Buoi08_A1");

        [Fact(DisplayName = "A1 · Ghi ds.txt rồi đọc lại có đánh số")]
        public void GhiDoc()
        {
            string d = BaiTap.ThuMucTam();
            string ra = b.ChayMainTrongThuMuc(d, "An\nBình\nChi\n\n");
            KiemTra.Chua(ra, "1. An", "Dòng 1");
            KiemTra.Chua(ra, "3. Chi", "Dòng 3");
            KiemTra.Dung(File.Exists(Path.Combine(d, "ds.txt")), "Phải tạo tệp ds.txt trong thư mục làm việc.");
            KiemTra.Bang(new[] { "An", "Bình", "Chi" }, File.ReadAllLines(Path.Combine(d, "ds.txt")), "Nội dung ds.txt");
        }
    }

    public class A2_ThongKeTep
    {
        readonly BaiTap b = new("Buoi08_A2");

        [Fact(DisplayName = "A2 · Tệp có dòng trống và dòng lỗi")]
        public void CoLoi()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "so.txt");
            File.WriteAllLines(tep, new[] { "7.5", "8", "", "abc", "6.5", "9" });
            b.Goi("ThongKeTep", tep);
            KiemTra.Chua(b.ManHinh, "7.75", "Trung bình (F2)");
            var so = KiemTra.CacSo(b.ManHinh);
            foreach (double x in new[] { 4.0, 1, 31, 9, 6.5 })
                KiemTra.Dung(so.Contains(x), $"Màn hình phải có số {x} (số hợp lệ 4, dòng lỗi 1, tổng 31, max 9, min 6.5).\nMàn hình:\n{b.ManHinh}");
        }

        [Fact(DisplayName = "A2 · Tệp rỗng")]
        public void Rong()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "rong.txt");
            File.WriteAllText(tep, "");
            b.Goi("ThongKeTep", tep);
            KiemTra.ChuaGanDung(b.ManHinh, "không có số hợp lệ", "Thông báo tệp rỗng");
        }

        [Fact(DisplayName = "A2 · Tệp chỉ có số âm")]
        public void SoAm()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "am.txt");
            File.WriteAllLines(tep, new[] { "-3", "-1.5", "-8" });
            b.Goi("ThongKeTep", tep);
            KiemTra.Dung(KiemTra.CacSo(b.ManHinh).Contains(-1.5), "max phải là -1.5 (khởi tạo max = double.MinValue, không phải 0).\nMàn hình:\n" + b.ManHinh);
        }
    }

    public class A3_NhatKy
    {
        readonly BaiTap b = new("Buoi08_A3");

        [Fact(DisplayName = "A3 · Chạy 4 lần: ghi thêm, in 3 dòng mới nhất")]
        public void BonLan()
        {
            string d = BaiTap.ThuMucTam();
            string ra = "";
            foreach (string s in new[] { "mot", "hai", "ba", "bon" }) ra = b.ChayMainTrongThuMuc(d, s + "\n");
            var dong = File.ReadAllLines(Path.Combine(d, "nhatky.txt"));
            KiemTra.Bang(4, dong.Length, "Số dòng trong nhatky.txt sau 4 lần chạy");
            KiemTra.Dung(dong[0].EndsWith("mot") && dong[3].EndsWith("bon"), "Các dòng phải được ghi thêm vào cuối theo thứ tự chạy.");
            string phanIn = ra.Substring(ra.IndexOf("bon\n") + 4);   // bỏ phần bàn phím vừa gõ
            KiemTra.Dung(!phanIn.Contains("mot") && phanIn.Contains("hai") && phanIn.Contains("bon"),
                "Lần chạy thứ 4 chỉ in 3 ghi chép mới nhất (hai, ba, bon).\nMàn hình:\n" + ra);
        }
    }

    public class A4_MaTranTep
    {
        readonly BaiTap b = new("Buoi08_A4");

        [Fact(DisplayName = "A4 · GhiMaTran đúng định dạng")]
        public void Ghi()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "mt.txt");
            b.Goi("GhiMaTran", tep, new[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            KiemTra.Bang("2 3\n1 2 3\n4 5 6", KiemTra.BoCuoiDong(File.ReadAllText(tep)), "Nội dung mt.txt");
        }

        [Fact(DisplayName = "A4 · DocMaTran chấp nhận nhiều dấu cách")]
        public void Doc()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "mt.txt");
            File.WriteAllText(tep, "2  3\n1   2 3\n 4 5    6\n");
            KiemTra.Bang(new[,] { { 1, 2, 3 }, { 4, 5, 6 } }, b.Goi("DocMaTran", tep), "DocMaTran");
        }
    }

    public class A5_DocAnToan
    {
        readonly BaiTap b = new("Buoi08_A5");

        [Fact(DisplayName = "A5 · Tệp không tồn tại → false, có thông báo")]
        public void KhongTonTai()
        {
            object[] ts = { Path.Combine(BaiTap.ThuMucTam(), "khong.txt"), null };
            KiemTra.Bang(false, b.Goi("DocMangAnToan", ts), "DocMangAnToan(khong.txt)");
            KiemTra.ChuaGanDung(b.ManHinh, "Không tìm thấy", "Thông báo");
        }

        [Fact(DisplayName = "A5 · Tệp có dòng không phải số → false")]
        public void SaiDinhDang()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "bad.txt");
            File.WriteAllLines(tep, new[] { "3", "x" });
            KiemTra.Bang(false, b.Goi("DocMangAnToan", tep, null), "DocMangAnToan(bad.txt)");
        }

        [Fact(DisplayName = "A5 · Tệp đúng → true và mảng {3, 1, 4}")]
        public void Dung()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "ok.txt");
            File.WriteAllLines(tep, new[] { "3", "1", "4" });
            object[] ts = { tep, null };
            KiemTra.Bang(true, b.Goi("DocMangAnToan", ts), "DocMangAnToan(ok.txt)");
            KiemTra.Bang(new[] { 3, 1, 4 }, ts[1], "Mảng đọc được");
        }
    }

    public class A6_BangDiemCSV
    {
        readonly BaiTap b = new("Buoi08_A6");

        [Fact(DisplayName = "A6 · GhiBangDiem rồi DocBangDiem")]
        public void GhiDoc()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "diem.csv");
            b.Goi("GhiBangDiem", tep, new[] { "SV01", "SV02" }, new double[,] { { 8, 7.5, 9 }, { 6, 5.5, 7 } });
            KiemTra.Bang("MSSV,Toan,Ly,Tin\nSV01,8,7.5,9\nSV02,6,5.5,7", KiemTra.BoCuoiDong(File.ReadAllText(tep)), "Nội dung diem.csv");
            object[] ts = { tep, null, null };
            b.Goi("DocBangDiem", ts);
            KiemTra.Bang(new[] { "SV01", "SV02" }, ts[1], "mssv đọc lại");
            KiemTra.Bang(new double[,] { { 8, 7.5, 9 }, { 6, 5.5, 7 } }, ts[2], "diem đọc lại");
        }
    }

    public class B1_DemTep
    {
        readonly BaiTap b = new("Buoi08_B1");

        [Fact(DisplayName = "B1 · 2 dòng, 8 từ, 32 ký tự")]
        public void Dem()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "vb.txt");
            File.WriteAllLines(tep, new[] { "Xin chào các bạn", "Học lập trình C#" });
            object[] ts = { tep, null, null, null };
            b.Goi("DemTep", ts);
            KiemTra.Bang(new[] { 2, 8, 32 }, new[] { (int)ts[1], (int)ts[2], (int)ts[3] }, "(số dòng, số từ, số ký tự)");
        }

        [Fact(DisplayName = "B1 · Nhiều dấu cách liền nhau")]
        public void NhieuCach()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "vb.txt");
            File.WriteAllLines(tep, new[] { "  a   b  " });
            object[] ts = { tep, null, null, null };
            b.Goi("DemTep", ts);
            KiemTra.Bang(2, ts[2], "Số từ của \"  a   b  \"");
        }
    }

    public class B2_SaoChepDanhSo
    {
        readonly BaiTap b = new("Buoi08_B2");

        [Fact(DisplayName = "B2 · Số thứ tự căn phải 3 ký tự")]
        public void DanhSo()
        {
            string d = BaiTap.ThuMucTam();
            string nguon = Path.Combine(d, "a.txt"), dich = Path.Combine(d, "b.txt");
            File.WriteAllLines(nguon, Enumerable.Range(1, 10).Select(i => "Dòng " + i));
            b.Goi("SaoChepDanhSo", nguon, dich);
            var kq = File.ReadAllLines(dich);
            KiemTra.Bang("  1: Dòng 1", kq[0], "Dòng đầu tệp đích");
            KiemTra.Bang(" 10: Dòng 10", kq[9], "Dòng 10 tệp đích");
        }
    }

    public class B3_LocNguyenTo
    {
        readonly BaiTap b = new("Buoi08_B3");

        [Fact(DisplayName = "B3 · Tệp 1..30 và một dòng sai → 10 số nguyên tố")]
        public void Loc()
        {
            string d = BaiTap.ThuMucTam();
            string nguon = Path.Combine(d, "so.txt"), dich = Path.Combine(d, "nguyento.txt");
            File.WriteAllLines(nguon, Enumerable.Range(1, 30).Select(i => i.ToString()).Append("abc"));
            KiemTra.Bang(10, b.Goi("LocSoNguyenTo", nguon, dich), "Số lượng");
            KiemTra.Bang(new[] { "2", "3", "5", "7", "11", "13", "17", "19", "23", "29" },
                File.ReadAllLines(dich).Select(s => s.Trim()).Where(s => s != "").ToArray(), "Nội dung nguyento.txt");
        }
    }

    public class B4_GopTep
    {
        readonly BaiTap b = new("Buoi08_B4");

        [Theory(DisplayName = "B4 · GopTep")]
        [InlineData("1 4 7 10", "2 3 8", "1 2 3 4 7 8 10")]
        [InlineData("", "2 3 8", "2 3 8")]
        [InlineData("5", "", "5")]
        public void Gop(string s1, string s2, string mongDoi)
        {
            string d = BaiTap.ThuMucTam();
            string t1 = Path.Combine(d, "1.txt"), t2 = Path.Combine(d, "2.txt"), kq = Path.Combine(d, "kq.txt");
            File.WriteAllLines(t1, s1.Split(' ', System.StringSplitOptions.RemoveEmptyEntries));
            File.WriteAllLines(t2, s2.Split(' ', System.StringSplitOptions.RemoveEmptyEntries));
            b.Goi("GopTep", t1, t2, kq);
            KiemTra.Bang(mongDoi, string.Join(" ", File.ReadAllLines(kq).Select(x => x.Trim()).Where(x => x != "")), $"Gộp \"{s1}\" và \"{s2}\"");
        }
    }

    public class B5_TanSuatChuCai
    {
        readonly BaiTap b = new("Buoi08_B5");

        [Fact(DisplayName = "B5 · DemChuCai(\"Hello World\")")]
        public void Dem()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "en.txt");
            File.WriteAllText(tep, "Hello World");
            var dem = (int[])b.Goi("DemChuCai", tep);
            KiemTra.Bang(26, dem.Length, "Độ dài mảng đếm");
            KiemTra.Bang(new[] { 1, 1, 1, 3, 2, 1, 1 }, new[] { dem['d' - 'a'], dem['e' - 'a'], dem['h' - 'a'], dem['l' - 'a'], dem['o' - 'a'], dem['r' - 'a'], dem['w' - 'a'] },
                "Số lần d, e, h, l, o, r, w");
            KiemTra.Bang(11 - 1, dem.Sum(), "Tổng số chữ cái (bỏ dấu cách)");
        }
    }

    public class B6_DanhBa
    {
        readonly BaiTap b = new("Buoi08_B6");

        [Theory(DisplayName = "B6 · LaSoDienThoaiHopLe")]
        [InlineData("0912345678", true)] [InlineData("091234567", false)] [InlineData("09123456a8", false)] [InlineData("09123456789", false)]
        public void SoDienThoai(string s, bool mongDoi) => KiemTra.Bang(mongDoi, b.Goi("LaSoDienThoaiHopLe", s), $"LaSoDienThoaiHopLe(\"{s}\")");

        [Fact(DisplayName = "B6 · GhiDanhBa rồi DocDanhBa")]
        public void GhiDoc()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "danhba.csv");
            string[] ten = new string[100], sdt = new string[100];
            ten[0] = "An"; sdt[0] = "0912345678"; ten[1] = "Bình"; sdt[1] = "0987654321";
            b.Goi("GhiDanhBa", tep, ten, sdt, 2);
            KiemTra.Bang("An,0912345678\nBình,0987654321", KiemTra.BoCuoiDong(File.ReadAllText(tep)), "Nội dung danhba.csv");
            string[] ten2 = new string[100], sdt2 = new string[100];
            KiemTra.Bang(2, b.Goi("DocDanhBa", tep, ten2, sdt2), "DocDanhBa trả về n");
            KiemTra.Bang(new[] { "Bình", "0987654321" }, new[] { ten2[1], sdt2[1] }, "Liên hệ thứ 2 đọc lại");
        }

        [Fact(DisplayName = "B6 · DocDanhBa khi chưa có tệp → 0")]
        public void ChuaCoTep() =>
            KiemTra.Bang(0, b.Goi("DocDanhBa", Path.Combine(BaiTap.ThuMucTam(), "khong.csv"), new string[100], new string[100]), "DocDanhBa(tệp không tồn tại)");
    }

    public class B7_Caesar
    {
        readonly BaiTap b = new("Buoi08_B7");

        [Theory(DisplayName = "B7 · MaHoaCaesar")]
        [InlineData("Hello, World!", 3, "Khoor, Zruog!")] [InlineData("xyz", 3, "abc")] [InlineData("Khoor, Zruog!", 23, "Hello, World!")]
        public void MaHoa(string s, int k, string mongDoi) => KiemTra.Bang(mongDoi, b.Goi("MaHoaCaesar", s, k), $"MaHoaCaesar(\"{s}\", {k})");

        [Fact(DisplayName = "B7 · MaHoaTep từng dòng")]
        public void MaHoaTep()
        {
            string d = BaiTap.ThuMucTam();
            string nguon = Path.Combine(d, "a.txt"), dich = Path.Combine(d, "b.txt");
            File.WriteAllLines(nguon, new[] { "Hello, World!", "xyz" });
            b.Goi("MaHoaTep", nguon, dich, 3);
            KiemTra.Bang(new[] { "Khoor, Zruog!", "abc" }, File.ReadAllLines(dich), "Nội dung tệp mã hóa");
        }
    }

    public class B8_BaoCaoBanHang
    {
        readonly BaiTap b = new("Buoi08_B8");

        [Fact(DisplayName = "B8 · Doanh thu theo sản phẩm")]
        public void BaoCao()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "banhang.csv");
            File.WriteAllLines(tep, new[] { "Ngay,SanPham,SoLuong,DonGia", "2026-10-01,But,10,5000", "2026-10-01,Vo,5,12000",
                                           "2026-10-02,But,4,5000", "2026-10-03,Thuoc,2,8000", "2026-10-03,Vo,3,12000" });
            object[] ts = { tep, null, null };
            b.Goi("BaoCaoBanHang", ts);
            KiemTra.Bang(new[] { "But", "Vo", "Thuoc" }, ts[1], "sanPham (theo thứ tự xuất hiện)");
            KiemTra.Bang(new long[] { 70000, 96000, 16000 }, ts[2], "doanhThu");
        }
    }
}
