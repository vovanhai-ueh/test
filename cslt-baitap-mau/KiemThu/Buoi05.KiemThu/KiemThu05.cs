using System;
using System.Linq;
using KiemThu.Chung;
using Xunit;

namespace KiemThu.Buoi05
{
    public class A1_TongDem
    {
        readonly BaiTap b = new("Buoi05_A1");

        [Theory(DisplayName = "A1 · TongDen (có trường hợp lặp 0 lần, 1 lần)")]
        [InlineData(4, 10)] [InlineData(100, 5050)] [InlineData(0, 0)] [InlineData(1, 1)]
        public void TongDen(int n, int mongDoi) => KiemTra.Bang(mongDoi, b.Goi("TongDen", n), $"TongDen({n})");

        [Theory(DisplayName = "A1 · DemSoChan(a, b)")]
        [InlineData(1, 10, 5)] [InlineData(3, 3, 0)] [InlineData(-4, 4, 5)] [InlineData(10, 1, 0)]
        public void DemSoChan(int a, int c, int mongDoi) => KiemTra.Bang(mongDoi, b.Goi("DemSoChan", a, c), $"DemSoChan({a}, {c})");
    }

    public class A2_NhapLai
    {
        readonly BaiTap b = new("Buoi05_A2");

        [Fact(DisplayName = "A2 · NhapSoNguyenTrongKhoang hỏi lại đến khi hợp lệ")]
        public void SoNguyen()
        {
            KiemTra.Bang(18, b.GoiCoNhap("NhapSoNguyenTrongKhoang", "abc\n0\n18.5\n18\n", "Nhập tuổi (1-120): ", 1, 120),
                "Bàn phím abc, 0, 18.5, 18");
            int soLanNhac = b.ManHinh.Split("Nhập tuổi").Length - 1;
            KiemTra.Bang(4, soLanNhac, "Số lần in lời nhắc (3 lần sai + 1 lần đúng)");
        }

        [Fact(DisplayName = "A2 · NhapSoThucTrongKhoang hỏi lại đến khi hợp lệ")]
        public void SoThuc() =>
            KiemTra.Bang(7.5, b.GoiCoNhap("NhapSoThucTrongKhoang", "-1\n10.5\n7.5\n", "Nhập điểm (0-10): ", 0.0, 10.0), "Bàn phím -1, 10.5, 7.5");

        [Fact(DisplayName = "A2 · Chấp nhận giá trị biên min, max")]
        public void Bien()
        {
            KiemTra.Bang(1, b.GoiCoNhap("NhapSoNguyenTrongKhoang", "1\n", "? ", 1, 120), "Nhập đúng min = 1");
            KiemTra.Bang(10.0, b.GoiCoNhap("NhapSoThucTrongKhoang", "10\n", "? ", 0.0, 10.0), "Nhập đúng max = 10");
        }
    }

    public class A3_XuLyChuSo
    {
        readonly BaiTap b = new("Buoi05_A3");

        [Theory(DisplayName = "A3 · DemChuSo, TongChuSo, DaoSo, LaSoDoiXung")]
        [InlineData(1234, 4, 10, 4321, false)] [InlineData(12321, 5, 9, 12321, true)] [InlineData(7, 1, 7, 7, true)]
        [InlineData(0, 1, 0, 0, true)] [InlineData(1200, 4, 3, 21, false)] [InlineData(905, 3, 14, 509, false)]
        public void ChuSo(int n, int dem, int tong, int dao, bool doiXung)
        {
            KiemTra.Bang(dem, b.Goi("DemChuSo", n), $"DemChuSo({n})");
            KiemTra.Bang(tong, b.Goi("TongChuSo", n), $"TongChuSo({n})");
            KiemTra.Bang(dao, b.Goi("DaoSo", n), $"DaoSo({n})");
            KiemTra.Bang(doiXung, b.Goi("LaSoDoiXung", n), $"LaSoDoiXung({n})");
        }
    }

    public class A4_InHinh
    {
        readonly BaiTap b = new("Buoi05_A4");

        [Fact(DisplayName = "A4 · InTamGiacVuong(4)")]
        public void TamGiac()
        {
            b.Goi("InTamGiacVuong", 4);
            KiemTra.HinhBang("*\n**\n***\n****", b.ManHinh, "InTamGiacVuong(4)");
        }

        [Fact(DisplayName = "A4 · InHinhChuNhatRong(6, 4)")]
        public void ChuNhat()
        {
            b.Goi("InHinhChuNhatRong", 6, 4);
            KiemTra.HinhBang("******\n*    *\n*    *\n******", b.ManHinh, "InHinhChuNhatRong(6, 4)");
        }

        [Fact(DisplayName = "A4 · InBangCuuChuong: 10 dòng, bảng 2–5")]
        public void CuuChuong()
        {
            b.Goi("InBangCuuChuong");
            var dong = KiemTra.BoCuoiDong(b.ManHinh).Split('\n');
            KiemTra.Bang(10, dong.Length, "Số dòng của bảng");
            KiemTra.Chua(b.ManHinh, "5 x  2 = 10", "Định dạng {bang} x {i,2} = {bang * i,2}");
            KiemTra.Chua(dong[9], "5 x 10 = 50", "Dòng cuối");
        }
    }

    public class A5_UCLNNguyenTo
    {
        readonly BaiTap b = new("Buoi05_A5");

        [Theory(DisplayName = "A5 · UCLN")]
        [InlineData(48, 18, 6)] [InlineData(17, 5, 1)] [InlineData(0, 7, 7)] [InlineData(-12, 18, 6)]
        public void UCLN(int a, int c, int mongDoi) => KiemTra.Bang(mongDoi, b.Goi("UCLN", a, c), $"UCLN({a}, {c})");

        [Theory(DisplayName = "A5 · BCNN")]
        [InlineData(4, 6, 12)] [InlineData(21, 6, 42)] [InlineData(7, 7, 7)]
        public void BCNN(int a, int c, int mongDoi) => KiemTra.Bang(mongDoi, b.Goi("BCNN", a, c), $"BCNN({a}, {c})");

        [Theory(DisplayName = "A5 · LaSoNguyenTo")]
        [InlineData(1, false)] [InlineData(2, true)] [InlineData(91, false)] [InlineData(97, true)] [InlineData(0, false)] [InlineData(-7, false)]
        public void NguyenTo(int n, bool mongDoi) => KiemTra.Bang(mongDoi, b.Goi("LaSoNguyenTo", n), $"LaSoNguyenTo({n})");

        [Fact(DisplayName = "A5 · LietKeNguyenTo(1, 50) in 15 số")]
        public void LietKe()
        {
            KiemTra.Bang(15, b.Goi("LietKeNguyenTo", 1, 50), "LietKeNguyenTo(1, 50) trả về");
            KiemTra.Chua(b.ManHinh, "2 3 5 7 11 13 17 19 23 29 31 37 41 43 47", "Dãy số in ra");
        }
    }

    public class A6_DoanSo
    {
        readonly BaiTap b = new("Buoi05_A6");

        [Fact(DisplayName = "A6 · Thắng sau 3 lượt (bí mật 42; đoán 50, 25, 42)")]
        public void Thang()
        {
            KiemTra.Bang(true, b.GoiCoNhap("ChoiMotVan", "50\n25\n42\n", 42, 7), "ChoiMotVan(42, 7)");
            KiemTra.ChuaGanDung(b.ManHinh, "Nhỏ hơn", "Gợi ý sau khi đoán 50");
            KiemTra.ChuaGanDung(b.ManHinh, "Lớn hơn", "Gợi ý sau khi đoán 25");
        }

        [Fact(DisplayName = "A6 · Thua khi hết 7 lượt")]
        public void Thua() =>
            KiemTra.Bang(false, b.GoiCoNhap("ChoiMotVan", string.Concat(Enumerable.Repeat("1\n", 7)), 42, 7), "ChoiMotVan(42, 7) đoán sai 7 lần");
    }

    public class B1_TongNghichDao
    {
        readonly BaiTap b = new("Buoi05_B1");

        [Theory(DisplayName = "B1 · TongNghichDao")]
        [InlineData(1, 1)] [InlineData(4, 2.083333333333333)] [InlineData(10, 2.9289682539682538)]
        public void Tong(int n, double mongDoi) => KiemTra.Gan(mongDoi, b.Goi<double>("TongNghichDao", n), 1e-12, $"TongNghichDao({n})");
    }

    public class B2_Uoc
    {
        readonly BaiTap b = new("Buoi05_B2");

        [Theory(DisplayName = "B2 · DemUoc và TongUoc")]
        [InlineData(12, 6, 28)] [InlineData(28, 6, 56)] [InlineData(7, 2, 8)] [InlineData(1, 1, 1)]
        public void Uoc(int n, int dem, int tong)
        {
            KiemTra.Bang(dem, b.Goi("DemUoc", n), $"DemUoc({n})");
            KiemTra.Bang(tong, b.Goi("TongUoc", n), $"TongUoc({n})");
        }
    }

    public class B3_SoHoanHao
    {
        readonly BaiTap b = new("Buoi05_B3");

        [Fact(DisplayName = "B3 · Số hoàn hảo trong [1, 10000] là 6 28 496 8128")]
        public void HoanHao()
        {
            var ds = Enumerable.Range(1, 10000).Where(n => (bool)b.Goi("LaSoHoanHao", n)).ToArray();
            KiemTra.Bang(new[] { 6, 28, 496, 8128 }, ds, "Các n trong [1, 10000] có LaSoHoanHao(n) = true");
        }
    }

    public class B4_KimTuThap
    {
        readonly BaiTap b = new("Buoi05_B4");

        [Fact(DisplayName = "B4 · InKimTuThapRong(5)")]
        public void N5()
        {
            b.Goi("InKimTuThapRong", 5);
            KiemTra.HinhBang("    *\n   * *\n  *   *\n *     *\n*********", b.ManHinh, "InKimTuThapRong(5)");
        }

        [Fact(DisplayName = "B4 · InKimTuThapRong(2)")]
        public void N2()
        {
            b.Goi("InKimTuThapRong", 2);
            KiemTra.HinhBang(" *\n***", b.ManHinh, "InKimTuThapRong(2)");
        }
    }

    public class B5_Fibonacci
    {
        readonly BaiTap b = new("Buoi05_B5");

        [Theory(DisplayName = "B5 · Fibonacci(k), F1 = F2 = 1")]
        [InlineData(1, 1L)] [InlineData(2, 1L)] [InlineData(10, 55L)] [InlineData(50, 12586269025L)]
        public void Fib(int k, long mongDoi) => KiemTra.Bang(mongDoi, b.Goi("Fibonacci", k), $"Fibonacci({k}) – dùng long để không tràn số");
    }

    public class B6_Newton
    {
        readonly BaiTap b = new("Buoi05_B6");

        [Theory(DisplayName = "B6 · CanBacHai (sai số ≤ 1e-12)")]
        [InlineData(2.0)] [InlineData(144.0)] [InlineData(0.25)] [InlineData(0.0)] [InlineData(1e6)]
        public void Can(double a) => KiemTra.Gan(Math.Sqrt(a), b.Goi<double>("CanBacHai", a), 1e-12 * Math.Max(1, Math.Sqrt(a)), $"CanBacHai({a})");
    }

    public class B7_ThuaSoNguyenTo
    {
        readonly BaiTap b = new("Buoi05_B7");

        [Theory(DisplayName = "B7 · PhanTichThuaSo")]
        [InlineData(360, "2 x 2 x 2 x 3 x 3 x 5")] [InlineData(97, "97")] [InlineData(1001, "7 x 11 x 13")]
        [InlineData(1024, "2 x 2 x 2 x 2 x 2 x 2 x 2 x 2 x 2 x 2")]
        public void PhanTich(int n, string mongDoi)
        {
            string kq = KiemTra.ChuanHoa(b.Goi<string>("PhanTichThuaSo", n)).Replace(" ", "").Replace("*", "x");
            string md = mongDoi.Replace(" ", "");
            KiemTra.Dung(kq == md || kq.EndsWith("=" + md), $"PhanTichThuaSo({n}): mong đợi \"{mongDoi}\", nhận được \"{kq}\".");
        }
    }

    public class B8_Armstrong
    {
        readonly BaiTap b = new("Buoi05_B8");

        [Fact(DisplayName = "B8 · Số Armstrong 3 chữ số là 153 370 371 407")]
        public void Armstrong()
        {
            var ds = Enumerable.Range(100, 900).Where(n => (bool)b.Goi("LaSoArmstrong", n)).ToArray();
            KiemTra.Bang(new[] { 153, 370, 371, 407 }, ds, "Các n trong [100, 999] có LaSoArmstrong(n) = true");
        }
    }
}
