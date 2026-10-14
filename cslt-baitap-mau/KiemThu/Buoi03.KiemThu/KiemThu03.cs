using System;
using KiemThu.Chung;
using Xunit;

namespace KiemThu.Buoi03
{
    public class A1_HamDauTien
    {
        readonly BaiTap b = new("Buoi03_A1");

        [Theory(DisplayName = "A1 · TinhDienTichHinhTron")]
        [InlineData(5, 78.53981633974483)]
        [InlineData(2, 12.566370614359172)]
        [InlineData(0, 0)]
        public void DienTich(double r, double mongDoi) =>
            KiemTra.Gan(mongDoi, b.Goi<double>("TinhDienTichHinhTron", r), 1e-9, $"TinhDienTichHinhTron({r})");

        [Fact(DisplayName = "A1 · InTieuDe in khung 30 dấu =")]
        public void InTieuDe()
        {
            b.Goi("InTieuDe", "XIN CHÀO");
            KiemTra.HinhBang(new string('=', 30) + "\nXIN CHÀO\n" + new string('=', 30), b.ManHinh, "InTieuDe(\"XIN CHÀO\")");
        }
    }

    public class A2_HamNhapLieu
    {
        readonly BaiTap b = new("Buoi03_A2");

        [Theory(DisplayName = "A2 · Chu vi, diện tích, đường chéo")]
        [InlineData(5, 3, 16, 15, 5.830951894845301)]
        [InlineData(2.5, 4, 13, 10, 4.716990566028302)]
        public void HinhChuNhat(double d, double r, double cv, double dt, double dc)
        {
            KiemTra.Gan(cv, b.Goi<double>("TinhChuVi", d, r), 1e-9, $"TinhChuVi({d}, {r})");
            KiemTra.Gan(dt, b.Goi<double>("TinhDienTich", d, r), 1e-9, $"TinhDienTich({d}, {r})");
            KiemTra.Gan(dc, b.Goi<double>("TinhDuongCheo", d, r), 1e-9, $"TinhDuongCheo({d}, {r})");
        }

        [Fact(DisplayName = "A2 · NhapSoThuc, NhapSoNguyen đọc từ bàn phím")]
        public void HamNhap()
        {
            KiemTra.Bang(2.5, b.GoiCoNhap("NhapSoThuc", "2.5\n", "Nhập: "), "NhapSoThuc với bàn phím \"2.5\"");
            KiemTra.Chua(b.ManHinh, "Nhập: ", "Hàm phải in lời nhắc");
            KiemTra.Bang(42, b.GoiCoNhap("NhapSoNguyen", "42\n", "Số: "), "NhapSoNguyen với bàn phím \"42\"");
        }
    }

    public class A3_HoaDonHam
    {
        readonly BaiTap b = new("Buoi03_A3");

        [Theory(DisplayName = "A3 · TinhThanhTien")]
        [InlineData(5, 15000, 75000)]
        [InlineData(10, 4500, 45000)]
        [InlineData(0, 8000, 0)]
        public void ThanhTien(int sl, int gia, int mongDoi) =>
            KiemTra.Bang((decimal)mongDoi, b.Goi("TinhThanhTien", sl, (decimal)gia), $"TinhThanhTien({sl}, {gia})");

        [Fact(DisplayName = "A3 · TinhThue 8%")]
        public void Thue() => KiemTra.Bang(10880m, b.Goi("TinhThue", 136000m), "TinhThue(136000)");
    }

    public class A4_ThamTriRef
    {
        readonly BaiTap b = new("Buoi03_A4");

        [Fact(DisplayName = "A4 · Tang(int) không làm đổi biến gọi")]
        public void Tang()
        {
            object[] ts = { 5 };
            b.Goi("Tang", ts);
            KiemTra.Bang(5, ts[0], "Sau Tang(a) với a = 5 (tham trị)");
        }

        [Fact(DisplayName = "A4 · TangRef(ref int)")]
        public void TangRef()
        {
            object[] ts = { 5 };
            b.Goi("TangRef", ts);
            KiemTra.Bang(6, ts[0], "Sau TangRef(ref a) với a = 5");
        }

        [Fact(DisplayName = "A4 · HoanDoi(ref, ref)")]
        public void HoanDoi()
        {
            object[] ts = { 3, 8 };
            b.Goi("HoanDoi", ts);
            KiemTra.Bang(new[] { 8, 3 }, new[] { (int)ts[0], (int)ts[1] }, "HoanDoi(ref x, ref y) với x = 3, y = 8");
        }
    }

    public class A5_ThamSoOut
    {
        readonly BaiTap b = new("Buoi03_A5");

        [Theory(DisplayName = "A5 · TachChuSo")]
        [InlineData(482, 4, 8, 2)]
        [InlineData(905, 9, 0, 5)]
        [InlineData(100, 1, 0, 0)]
        public void TachChuSo(int n, int tram, int chuc, int dv)
        {
            object[] ts = { n, null, null, null };
            b.Goi("TachChuSo", ts);
            KiemTra.Bang(new[] { tram, chuc, dv }, new[] { (int)ts[1], (int)ts[2], (int)ts[3] }, $"TachChuSo({n}) → (trăm, chục, đơn vị)");
        }
    }

    public class A6_NapChong
    {
        readonly BaiTap b = new("Buoi03_A6");

        [Fact(DisplayName = "A6 · Ba hàm DienTich nạp chồng")]
        public void DienTich()
        {
            KiemTra.Gan(16, b.Goi<double>("DienTich", 4.0), 1e-9, "DienTich(4)");
            KiemTra.Gan(15, b.Goi<double>("DienTich", 5.0, 3.0), 1e-9, "DienTich(5, 3)");
            KiemTra.Gan(6, b.Goi<double>("DienTich", 3.0, 4.0, 5.0), 1e-9, "DienTich(3, 4, 5) – Heron");
        }

        [Fact(DisplayName = "A6 · TinhTienLai có tham số mặc định 0.06 và 1")]
        public void TienLai()
        {
            KiemTra.Gan(6_000_000, b.Goi<double>("TinhTienLai", 100_000_000.0, 0.06, 1), 1e-3, "TinhTienLai(100000000, 0.06, 1)");
            KiemTra.Gan(33_822_557.76, b.Goi<double>("TinhTienLai", 100_000_000.0, 0.06, 5), 0.01, "TinhTienLai(100000000, 0.06, 5)");
            KiemTra.Gan(24_229_687.5, b.Goi<double>("TinhTienLai", 100_000_000.0, 0.075, 3), 0.01, "TinhTienLai(100000000, 0.075, 3)");
            ThamSoMacDinh.KiemTra(b, "TinhTienLai", 3, new object[] { null, 0.06, 1 });
        }
    }

    public class B1_DoiNhietDo
    {
        readonly BaiTap b = new("Buoi03_B1");

        [Theory(DisplayName = "B1 · DoiCsangF")]
        [InlineData(37, 98.6)]
        [InlineData(100, 212)]
        [InlineData(-40, -40)]
        public void CsangF(double c, double f) => KiemTra.Gan(f, b.Goi<double>("DoiCsangF", c), 1e-9, $"DoiCsangF({c})");

        [Theory(DisplayName = "B1 · DoiFsangC")]
        [InlineData(98.6, 37)]
        [InlineData(32, 0)]
        public void FsangC(double f, double c) => KiemTra.Gan(c, b.Goi<double>("DoiFsangC", f), 1e-9, $"DoiFsangC({f})");
    }

    public class B2_HamBool
    {
        readonly BaiTap b = new("Buoi03_B2");

        [Theory(DisplayName = "B2 · LaNamNhuan")]
        [InlineData(2024, true)] [InlineData(1900, false)] [InlineData(2000, true)] [InlineData(2023, false)]
        public void NamNhuan(int nam, bool mongDoi) => KiemTra.Bang(mongDoi, b.Goi("LaNamNhuan", nam), $"LaNamNhuan({nam})");

        [Theory(DisplayName = "B2 · LaTamGiac")]
        [InlineData(3, 4, 5, true)] [InlineData(1, 2, 3, false)] [InlineData(2, 2, 3, true)] [InlineData(-3, 4, 5, false)]
        public void TamGiac(double a, double c, double d, bool mongDoi) =>
            KiemTra.Bang(mongDoi, b.Goi("LaTamGiac", a, c, d), $"LaTamGiac({a}, {c}, {d})");
    }

    public class B3_KhoangCach
    {
        readonly BaiTap b = new("Buoi03_B3");

        [Fact(DisplayName = "B3 · KhoangCach")]
        public void KhoangCach() => KiemTra.Gan(5, b.Goi<double>("KhoangCach", 0.0, 0.0, 3.0, 4.0), 1e-9, "KhoangCach(0, 0, 3, 4)");

        [Fact(DisplayName = "B3 · ChuViTamGiac A(0,0) B(3,0) C(0,4)")]
        public void ChuVi() => KiemTra.Gan(12, b.Goi<double>("ChuViTamGiac", 0.0, 0.0, 3.0, 0.0, 0.0, 4.0), 1e-9, "ChuViTamGiac(0, 0, 3, 0, 0, 4)");
    }

    public class B4_TongChuSoSoDao
    {
        readonly BaiTap b = new("Buoi03_B4");

        [Theory(DisplayName = "B4 · TongChuSo3 và SoDao3")]
        [InlineData(482, 14, 284)] [InlineData(905, 14, 509)] [InlineData(100, 1, 1)]
        public void TongVaDao(int n, int tong, int dao)
        {
            KiemTra.Bang(tong, b.Goi("TongChuSo3", n), $"TongChuSo3({n})");
            KiemTra.Bang(dao, b.Goi("SoDao3", n), $"SoDao3({n})");
        }
    }

    public class B5_XoayVong
    {
        readonly BaiTap b = new("Buoi03_B5");

        [Theory(DisplayName = "B5 · XoayVong(ref a, ref b, ref c)")]
        [InlineData(1, 2, 3)] [InlineData(7, 7, 9)]
        public void XoayVong(int x, int y, int z)
        {
            object[] ts = { x, y, z };
            b.Goi("XoayVong", ts);
            KiemTra.Bang(new[] { y, z, x }, new[] { (int)ts[0], (int)ts[1], (int)ts[2] }, $"XoayVong({x}, {y}, {z})");
        }
    }

    public class B6_PTBac2Out
    {
        readonly BaiTap b = new("Buoi03_B6");

        [Theory(DisplayName = "B6 · GiaiPTBac2 với out x1, x2")]
        [InlineData(1, -3, 2, 2, 1)] [InlineData(1, 2, 1, -1, -1)] [InlineData(2, -2, -4, 2, -1)]
        public void PTBac2(double a, double c, double d, double x1, double x2)
        {
            object[] ts = { a, c, d, null, null };
            b.Goi("GiaiPTBac2", ts);
            KiemTra.Gan(x1, (double)ts[3], 1e-9, $"GiaiPTBac2({a}, {c}, {d}) → x1");
            KiemTra.Gan(x2, (double)ts[4], 1e-9, $"GiaiPTBac2({a}, {c}, {d}) → x2");
        }
    }

    public class B7_SoNgayGiua
    {
        readonly BaiTap b = new("Buoi03_B7");

        [Theory(DisplayName = "B7 · SoNgayGiua")]
        [InlineData(13, 10, 2026, 1, 1, 2027, 80)]
        [InlineData(1, 1, 2000, 1, 1, 2026, 9497)]
        [InlineData(2, 9, 1945, 2, 9, 2026, 29585)]
        [InlineData(28, 2, 2024, 1, 3, 2024, 2)]
        public void SoNgay(int d1, int m1, int y1, int d2, int m2, int y2, int mongDoi) =>
            KiemTra.Bang(mongDoi, b.Goi("SoNgayGiua", d1, m1, y1, d2, m2, y2), $"SoNgayGiua({d1}/{m1}/{y1} → {d2}/{m2}/{y2})");

        [Fact(DisplayName = "B7 · SoNgayJulian(1, 1, 2000) = 2451545")]
        public void Julian() => KiemTra.Bang(2451545, b.Goi("SoNgayJulian", 1, 1, 2000), "SoNgayJulian(1, 1, 2000)");
    }

    public class B8_TinhLuong
    {
        readonly BaiTap b = new("Buoi03_B8");

        [Fact(DisplayName = "B8 · Lương 5,000,000 × 2.34 × 24/26")]
        public void Luong()
        {
            double lt = b.Goi<double>("TinhLuongThang", 5_000_000.0, 2.34, 24);
            KiemTra.Gan(10_800_000, lt, 0.5, "TinhLuongThang(5000000, 2.34, 24)");
            KiemTra.Gan(1_134_000, b.Goi<double>("TinhBaoHiem", 10_800_000.0), 0.5, "TinhBaoHiem(10800000)");
            KiemTra.Gan(9_666_000, b.Goi<double>("TinhThucLinh", 10_800_000.0), 0.5, "TinhThucLinh(10800000)");
        }
    }

    internal static class ThamSoMacDinh
    {
        public static void KiemTra(BaiTap b, string ten, int soThamSo, object[] macDinh)
        {
            var asm = System.Reflection.Assembly.Load(b.Ten);
            foreach (var t in asm.GetTypes())
                foreach (var m in t.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
                    if (m.Name == ten && m.GetParameters().Length == soThamSo)
                    {
                        var ps = m.GetParameters();
                        for (int i = 0; i < soThamSo; i++)
                            if (macDinh[i] != null)
                                Chung.KiemTra.Dung(ps[i].HasDefaultValue && Chung.KiemTra.GiongNhau(macDinh[i], ps[i].DefaultValue),
                                    $"Tham số thứ {i + 1} của {ten} phải có giá trị mặc định {macDinh[i]}.");
                        return;
                    }
        }
    }
}
