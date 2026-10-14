using KiemThu.Chung;
using Xunit;

namespace KiemThu.Buoi04
{
    public class A1_PhanLoaiSo
    {
        readonly BaiTap b = new("Buoi04_A1");

        [Theory(DisplayName = "A1 · PhanLoaiSo")]
        [InlineData(7, "7 là số dương, lẻ")]
        [InlineData(-4, "-4 là số âm, chẵn")]
        [InlineData(0, "0 là số bằng 0, chẵn")]
        [InlineData(-3, "-3 là số âm, lẻ")]
        public void PhanLoai(int n, string mongDoi) => KiemTra.ChuoiBang(mongDoi, b.Goi<string>("PhanLoaiSo", n), $"PhanLoaiSo({n})");
    }

    public class A2_XepLoai
    {
        readonly BaiTap b = new("Buoi04_A2");

        [Theory(DisplayName = "A2 · XepLoai (có giá trị biên)")]
        [InlineData(9, "Giỏi")] [InlineData(8, "Giỏi")] [InlineData(7.9, "Khá")] [InlineData(6.5, "Khá")]
        [InlineData(5, "Trung bình")] [InlineData(4.9, "Yếu")] [InlineData(0, "Yếu")]
        public void XepLoai(double diem, string mongDoi) => KiemTra.ChuoiBang(mongDoi, b.Goi<string>("XepLoai", diem), $"XepLoai({diem})");

        [Theory(DisplayName = "A2 · Main báo dữ liệu không hợp lệ")]
        [InlineData("abc")] [InlineData("-1")] [InlineData("10.5")]
        public void KhongHopLe(string nhap) => KiemTra.ChuaGanDung(b.ChayMain(nhap + "\n"), "Dữ liệu không hợp lệ", $"Nhập \"{nhap}\"");

        [Fact(DisplayName = "A2 · Main in xếp loại khi điểm hợp lệ")]
        public void HopLe() => KiemTra.ChuaGanDung(b.ChayMain("6.5\n"), "Khá", "Nhập 6.5");
    }

    public class A3_PhuongTrinh
    {
        readonly BaiTap b = new("Buoi04_A3");

        [Theory(DisplayName = "A3 · GiaiPTBac2 đủ 6 nhánh")]
        [InlineData(1, -3, 2, "x1 = 2, x2 = 1")]
        [InlineData(1, 2, 1, "Nghiệm kép x = -1")]
        [InlineData(1, 0, 1, "Vô nghiệm")]
        [InlineData(0, 2, -4, "x = 2")]
        [InlineData(0, 0, 0, "Vô số nghiệm")]
        [InlineData(0, 0, 5, "Vô nghiệm")]
        public void Bac2(double a, double c, double d, string mongDoi) =>
            KiemTra.ChuoiBang(mongDoi, b.Goi<string>("GiaiPTBac2", a, c, d), $"GiaiPTBac2({a}, {c}, {d})");

        [Theory(DisplayName = "A3 · GiaiPTBac1")]
        [InlineData(2, -4, "x = 2")] [InlineData(0, 0, "Vô số nghiệm")] [InlineData(0, 3, "Vô nghiệm")]
        public void Bac1(double a, double c, string mongDoi) =>
            KiemTra.ChuoiBang(mongDoi, b.Goi<string>("GiaiPTBac1", a, c), $"GiaiPTBac1({a}, {c})");
    }

    public class A4_SwitchNgayThang
    {
        readonly BaiTap b = new("Buoi04_A4");

        [Theory(DisplayName = "A4 · SoNgayTrongThang")]
        [InlineData(1, 2026, 31)] [InlineData(4, 2026, 30)] [InlineData(2, 2024, 29)] [InlineData(2, 2026, 28)]
        [InlineData(2, 1900, 28)] [InlineData(2, 2000, 29)] [InlineData(13, 2026, -1)]
        public void SoNgay(int thang, int nam, int mongDoi) =>
            KiemTra.Bang(mongDoi, b.Goi("SoNgayTrongThang", thang, nam), $"SoNgayTrongThang({thang}, {nam})");

        [Theory(DisplayName = "A4 · TenThu")]
        [InlineData(0, "Chủ nhật")] [InlineData(2, "Thứ Ba")] [InlineData(6, "Thứ Bảy")] [InlineData(7, "Không hợp lệ")]
        public void TenThu(int thu, string mongDoi) => KiemTra.ChuoiBang(mongDoi, b.Goi<string>("TenThu", thu), $"TenThu({thu})");

        [Theory(DisplayName = "A4 · LaNgayHopLe")]
        [InlineData(29, 2, 2024, true)] [InlineData(29, 2, 2026, false)] [InlineData(31, 4, 2026, false)]
        [InlineData(0, 1, 2026, false)] [InlineData(1, 13, 2026, false)] [InlineData(31, 12, 2026, true)]
        public void HopLe(int d, int m, int y, bool mongDoi) =>
            KiemTra.Bang(mongDoi, b.Goi("LaNgayHopLe", d, m, y), $"LaNgayHopLe({d}, {m}, {y})");

        [Theory(DisplayName = "A4 · ThuTrongTuan")]
        [InlineData(13, 10, 2026, 2)] [InlineData(2, 9, 1945, 0)] [InlineData(29, 2, 2024, 4)]
        public void Thu(int d, int m, int y, int mongDoi) =>
            KiemTra.Bang(mongDoi, b.Goi("ThuTrongTuan", d, m, y), $"ThuTrongTuan({d}, {m}, {y})");
    }

    public class A5_MayTinh
    {
        readonly BaiTap b = new("Buoi04_A5");

        [Theory(DisplayName = "A5 · TinhToan")]
        [InlineData(8, '/', 2, "8 / 2 = 4")]
        [InlineData(8, '/', 0, "Lỗi: không chia được cho 0")]
        [InlineData(2, '^', 10, "2 ^ 10 = 1024")]
        [InlineData(7, '%', 3, "7 % 3 = 1")]
        [InlineData(7, '%', 0, "Lỗi: không chia được cho 0")]
        [InlineData(3, '+', 4.5, "3 + 4.5 = 7.5")]
        [InlineData(3, '-', 5, "3 - 5 = -2")]
        [InlineData(6, '*', 7, "6 * 7 = 42")]
        [InlineData(8, '#', 2, "Phép toán '#' không được hỗ trợ")]
        public void TinhToan(double a, char p, double c, string mongDoi) =>
            KiemTra.ChuoiBang(mongDoi, b.Goi<string>("TinhToan", a, p, c), $"TinhToan({a}, '{p}', {c})");
    }

    public class A6_TryParse
    {
        readonly BaiTap b = new("Buoi04_A6");

        [Theory(DisplayName = "A6 · NhapSoNguyen(loiNhac, out ketQua)")]
        [InlineData("2006", true, 2006)] [InlineData("abc", false, 0)] [InlineData("", false, 0)] [InlineData("-15", true, -15)]
        public void Nhap(string phim, bool hopLe, int giaTri)
        {
            object[] ts = { "Nhập năm sinh: ", null };
            KiemTra.Bang(hopLe, b.GoiCoNhap("NhapSoNguyen", phim + "\n", ts), $"NhapSoNguyen với bàn phím \"{phim}\"");
            if (hopLe) KiemTra.Bang(giaTri, ts[1], $"Giá trị out khi nhập \"{phim}\"");
        }

        [Fact(DisplayName = "A6 · TinhTuoi")]
        public void Tuoi() => KiemTra.Bang(20, b.Goi("TinhTuoi", 2006, 2026), "TinhTuoi(2006, 2026)");
    }

    public class B1_Max3Min3
    {
        readonly BaiTap b = new("Buoi04_B1");

        [Theory(DisplayName = "B1 · TimMax3 và TimMin3")]
        [InlineData(3, 9, 5, 9, 3)] [InlineData(9, 3, 5, 9, 3)] [InlineData(5, 3, 9, 9, 3)]
        [InlineData(4, 4, 4, 4, 4)] [InlineData(-1, -5, -3, -1, -5)]
        public void MaxMin(int x, int y, int z, int max, int min)
        {
            KiemTra.Bang(max, b.Goi("TimMax3", x, y, z), $"TimMax3({x}, {y}, {z})");
            KiemTra.Bang(min, b.Goi("TimMin3", x, y, z), $"TimMin3({x}, {y}, {z})");
        }
    }

    public class B2_PhanLoaiBMI
    {
        readonly BaiTap b = new("Buoi04_B2");

        [Theory(DisplayName = "B2 · TinhBMI rồi PhanLoaiBMI (cao 1.7 m)")]
        [InlineData(45, "Gầy")] [InlineData(60, "Bình thường")] [InlineData(80, "Thừa cân")] [InlineData(95, "Béo phì")]
        public void BMI(double kg, string mongDoi)
        {
            double bmi = b.Goi<double>("TinhBMI", kg, 1.7);
            KiemTra.Gan(kg / (1.7 * 1.7), bmi, 1e-9, $"TinhBMI({kg}, 1.7)");
            KiemTra.ChuoiBang(mongDoi, b.Goi<string>("PhanLoaiBMI", bmi), $"PhanLoaiBMI({bmi:F2})");
        }

        [Theory(DisplayName = "B2 · PhanLoaiBMI tại giá trị biên")]
        [InlineData(18.5, "Bình thường")] [InlineData(25, "Thừa cân")] [InlineData(30, "Béo phì")] [InlineData(18.49, "Gầy")]
        public void Bien(double bmi, string mongDoi) => KiemTra.ChuoiBang(mongDoi, b.Goi<string>("PhanLoaiBMI", bmi), $"PhanLoaiBMI({bmi})");
    }

    public class B3_TienDien
    {
        readonly BaiTap b = new("Buoi04_B3");

        [Theory(DisplayName = "B3 · TinhTienDien bậc thang")]
        [InlineData(45, 81000)] [InlineData(150, 295000)] [InlineData(450, 1155000)]
        [InlineData(0, 0)] [InlineData(50, 90000)] [InlineData(100, 185000)] [InlineData(400, 995000)]
        public void TienDien(int kwh, long mongDoi) => KiemTra.Bang(mongDoi, b.Goi("TinhTienDien", kwh), $"TinhTienDien({kwh})");
    }

    public class B4_PhanLoaiTamGiac
    {
        readonly BaiTap b = new("Buoi04_B4");

        [Theory(DisplayName = "B4 · PhanLoaiTamGiac")]
        [InlineData(1, 2, 3, "Không phải tam giác")] [InlineData(3, 3, 3, "Đều")] [InlineData(3, 4, 5, "Vuông")]
        [InlineData(5, 3, 4, "Vuông")] [InlineData(3, 3, 4, "Cân")] [InlineData(2, 3, 4, "Thường")]
        [InlineData(1, 1, 1.4142135623730951, "Vuông cân")]
        public void PhanLoai(double x, double y, double z, string mongDoi) =>
            KiemTra.ChuoiBang(mongDoi, b.Goi<string>("PhanLoaiTamGiac", x, y, z), $"PhanLoaiTamGiac({x}, {y}, {z})");
    }

    public class B5_CuocTaxi
    {
        readonly BaiTap b = new("Buoi04_B5");

        [Theory(DisplayName = "B5 · TinhCuocTaxi")]
        [InlineData(1, 12000)] [InlineData(10, 102000)] [InlineData(30, 302000)] [InlineData(40, 382000)] [InlineData(31, 310000)]
        public void Cuoc(int km, long mongDoi) => KiemTra.Bang(mongDoi, b.Goi("TinhCuocTaxi", km), $"TinhCuocTaxi({km})");
    }

    public class B6_NgayHomSau
    {
        readonly BaiTap b = new("Buoi04_B6");

        [Theory(DisplayName = "B6 · NgayHomSau(ref ngay, ref thang, ref nam)")]
        [InlineData(13, 10, 2026, "14/10/2026")] [InlineData(31, 10, 2026, "1/11/2026")] [InlineData(28, 2, 2024, "29/2/2024")]
        [InlineData(28, 2, 2026, "1/3/2026")] [InlineData(31, 12, 2026, "1/1/2027")] [InlineData(29, 2, 2024, "1/3/2024")]
        public void HomSau(int d, int m, int y, string mongDoi)
        {
            object[] ts = { d, m, y };
            b.Goi("NgayHomSau", ts);
            KiemTra.Bang(mongDoi, $"{ts[0]}/{ts[1]}/{ts[2]}", $"Ngày sau {d}/{m}/{y}");
        }
    }

    public class B7_HePhuongTrinh
    {
        readonly BaiTap b = new("Buoi04_B7");

        [Fact(DisplayName = "B7 · Nghiệm duy nhất x = 2, y = 1")]
        public void DuyNhat()
        {
            string kq = KiemTra.ChuanHoa(b.Goi<string>("GiaiHePT", 1.0, 1.0, 3.0, 1.0, -1.0, 1.0)).Replace(" ", "");
            KiemTra.Dung(kq.Contains("x=2") && kq.Contains("y=1"), $"GiaiHePT(1, 1, 3, 1, -1, 1) phải chứa \"x = 2\" và \"y = 1\", nhận được \"{kq}\".");
        }

        [Theory(DisplayName = "B7 · Vô số nghiệm / vô nghiệm")]
        [InlineData(1, 1, 2, 2, 2, 4, "Vô số nghiệm")] [InlineData(1, 1, 2, 1, 1, 3, "Vô nghiệm")]
        public void DacBiet(double a1, double b1, double c1, double a2, double b2, double c2, string mongDoi) =>
            KiemTra.ChuoiBang(mongDoi, b.Goi<string>("GiaiHePT", a1, b1, c1, a2, b2, c2), $"GiaiHePT({a1}, {b1}, {c1}, {a2}, {b2}, {c2})");
    }

    public class B8_DocSo
    {
        readonly BaiTap b = new("Buoi04_B8");

        [Theory(DisplayName = "B8 · DocSo3ChuSo")]
        [InlineData(482, "bốn trăm tám mươi hai")] [InlineData(105, "một trăm linh năm")] [InlineData(115, "một trăm mười lăm")]
        [InlineData(121, "một trăm hai mươi mốt")] [InlineData(300, "ba trăm")] [InlineData(210, "hai trăm mười")]
        [InlineData(999, "chín trăm chín mươi chín")]
        public void DocSo(int n, string mongDoi) => KiemTra.ChuoiBang(mongDoi, b.Goi<string>("DocSo3ChuSo", n), $"DocSo3ChuSo({n})");

        [Fact(DisplayName = "B8 · 444 đọc \"tư\" hoặc \"bốn\" ở hàng đơn vị")]
        public void BonTramBonMuoiTu()
        {
            string kq = KiemTra.ChuanHoa(b.Goi<string>("DocSo3ChuSo", 444));
            KiemTra.Dung(kq == "bốn trăm bốn mươi tư" || kq == "bốn trăm bốn mươi bốn", $"DocSo3ChuSo(444): nhận được \"{kq}\".");
        }
    }
}
