using System.Linq;
using KiemThu.Chung;
using Xunit;

// Buổi 2: chạy Main với dữ liệu nhập giả lập, tìm các con số trong màn hình.
namespace KiemThu.Buoi02
{
    public class A1_GioiHanKieuSo
    {
        readonly BaiTap b = new("Buoi02_A1");

        [Fact(DisplayName = "A1 · Tràn số int và định dạng N0")]
        public void TranSo()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "2147483647", "int.MaxValue");
            KiemTra.Chua(ra, "-2147483648", "int.MaxValue + 1 bị tràn");
            KiemTra.Chua(ra, "8,100,000,000", "Dân số in bằng {danSo:N0}");
        }
    }

    public class A2_HinhChuNhat
    {
        readonly BaiTap b = new("Buoi02_A2");

        [Theory(DisplayName = "A2 · Chu vi, diện tích, đường chéo (F2)")]
        [InlineData("5\n3\n", "16.00", "15.00", "5.83")]
        [InlineData("2.5\n4\n", "13.00", "10.00", "4.72")]
        public void TinhToan(string nhap, string cv, string dt, string dc)
        {
            string ra = b.ChayMain(nhap);
            KiemTra.Chua(ra, cv, "Chu vi");
            KiemTra.Chua(ra, dt, "Diện tích");
            KiemTra.Chua(ra, dc, "Đường chéo");
        }
    }

    public class A3_DoiGiay
    {
        readonly BaiTap b = new("Buoi02_A3");

        [Theory(DisplayName = "A3 · Dạng đồng hồ hh:mm:ss")]
        [InlineData("3725", "01:02:05")]
        [InlineData("59", "00:00:59")]
        [InlineData("3600", "01:00:00")]
        [InlineData("86399", "23:59:59")]
        public void DongHo(string nhap, string mongDoi) => KiemTra.Chua(b.ChayMain(nhap + "\n"), mongDoi, $"Nhập {nhap}");

        [Fact(DisplayName = "A3 · 3725 giây = 1 giờ 2 phút 5 giây")]
        public void DangChu() => KiemTra.ChuaGanDung(b.ChayMain("3725\n"), "1 giờ 2 phút 5 giây", "Nhập 3725");
    }

    public class A4_HoaDon
    {
        readonly BaiTap b = new("Buoi02_A4");

        [Fact(DisplayName = "A4 · Cộng 136,000; thuế 10,880; thanh toán 146,880")]
        public void HoaDon()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "136,000", "Dòng Cộng");
            KiemTra.Chua(ra, "10,880", "Dòng thuế VAT");
            KiemTra.Chua(ra, "146,880", "Dòng THANH TOÁN");
            KiemTra.Chua(ra, "Vở 200 trang       5      15,000        75,000", "Dòng hàng thẳng cột {ten,-16}{sl,4}{gia,12:N0}{tt,14:N0}");
        }
    }

    public class A5_BayChiaNguyen
    {
        readonly BaiTap b = new("Buoi02_A5");

        [Fact(DisplayName = "A5 · Kết quả 10 phép tính")]
        public void MuoiPhepTinh()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "3.5", "(double)a / b");
            KiemTra.Chua(ra, "0.30000000000000004", "0.1 + 0.2");
            KiemTra.Chua(ra, "Infinity", "1 / 0.0");
        }
    }

    public class A6_BieuThucLogic
    {
        readonly BaiTap b = new("Buoi02_A6");

        [Theory(DisplayName = "A6 · Năm nhuận rồi tam giác (True/False)")]
        [InlineData("2024\n3\n4\n5\n", "True", "True")]
        [InlineData("1900\n1\n2\n3\n", "False", "False")]
        [InlineData("2000\n2\n2\n3\n", "True", "True")]
        [InlineData("2023\n3\n4\n5\n", "False", "True")]
        public void NamNhuanTamGiac(string nhap, string nhuan, string tamGiac)
        {
            string ra = b.ChayMain(nhap);
            var tu = System.Text.RegularExpressions.Regex.Matches(ra, "True|False").Select(m => m.Value).ToList();
            KiemTra.Dung(tu.Count >= 2 && tu[^2] == nhuan && tu[^1] == tamGiac,
                $"Nhập {nhap.Replace("\n", " ").Trim()}: mong đợi {nhuan} rồi {tamGiac}.\nMàn hình:\n{ra}");
        }
    }

    public class B1_DoiNhietDo
    {
        readonly BaiTap b = new("Buoi02_B1");

        [Theory(DisplayName = "B1 · Độ F và độ K (F2)")]
        [InlineData("37", "98.60", "310.15")]
        [InlineData("100", "212.00", "373.15")]
        [InlineData("-40", "-40.00", "233.15")]
        public void NhietDo(string c, string f, string k)
        {
            string ra = b.ChayMain(c + "\n");
            KiemTra.Chua(ra, f, $"{c} °C sang °F");
            KiemTra.Chua(ra, k, $"{c} °C sang K");
        }
    }

    public class B2_BMI
    {
        readonly BaiTap b = new("Buoi02_B2");

        [Theory(DisplayName = "B2 · BMI (F2)")]
        [InlineData("60\n1.7\n", "20.76")]
        [InlineData("75\n1.75\n", "24.49")]
        public void BMI(string nhap, string mongDoi) => KiemTra.Chua(b.ChayMain(nhap), mongDoi, "BMI");
    }

    public class B3_KhoangCach
    {
        readonly BaiTap b = new("Buoi02_B3");

        [Theory(DisplayName = "B3 · Độ dài AB (nhập x1, y1, x2, y2)")]
        [InlineData("1\n2\n4\n6\n", "5.00")]
        [InlineData("0\n0\n1\n1\n", "1.41")]
        public void AB(string nhap, string mongDoi) => KiemTra.Chua(b.ChayMain(nhap), mongDoi, "AB");
    }

    public class B4_TachChuSo
    {
        readonly BaiTap b = new("Buoi02_B4");

        [Theory(DisplayName = "B4 · Tổng chữ số và số đảo")]
        [InlineData("482", 14, 284)]
        [InlineData("905", 14, 509)]
        public void TachSo(string n, int tong, int dao)
        {
            var so = KiemTra.CacSo(b.ChayMain(n + "\n"));
            KiemTra.Dung(so.Contains(tong), $"Nhập {n}: phải in tổng các chữ số {tong}.");
            KiemTra.Dung(so.Contains(dao), $"Nhập {n}: phải in số đảo {dao}.");
        }
    }

    public class B5_HoanDoi
    {
        readonly BaiTap b = new("Buoi02_B5");

        [Theory(DisplayName = "B5 · Hai số cuối in ra là a, b sau hoán đổi")]
        [InlineData(3, 8)]
        [InlineData(-5, 12)]
        public void HoanDoi(int a, int c)
        {
            string ra = b.ChayMain($"{a}\n{c}\n");
            var so = KiemTra.CacSo(ra);
            KiemTra.Dung(so.Count >= 2 && so[^2] == c && so[^1] == a,
                $"Nhập a = {a}, b = {c}: dòng cuối phải là a = {c}, b = {a}.\nMàn hình:\n{ra}");
        }
    }

    public class B6_LaiKep
    {
        readonly BaiTap b = new("Buoi02_B6");

        [Fact(DisplayName = "B6 · P = 100,000,000; r = 6; n = 5")]
        public void LaiKep()
        {
            string ra = b.ChayMain("100000000\n6\n5\n");
            KiemTra.Chua(ra, "133,822,557.76", "Số tiền nhận (N2)");
            KiemTra.Chua(ra, "33,822,557.76", "Tiền lãi (N2)");
        }
    }

    public class B7_LamTron
    {
        readonly BaiTap b = new("Buoi02_B7");

        [Theory(DisplayName = "B7 · Làm tròn đến hàng nghìn")]
        [InlineData(146880, 147000)]
        [InlineData(12499, 12000)]
        [InlineData(12500, 13000)]
        [InlineData(999, 1000)]
        public void LamTron(int n, int mongDoi) => KiemTra.SoCuoi(b.ChayMain(n + "\n"), mongDoi, $"Nhập {n}");
    }

    public class B8_ThuTrongTuan
    {
        readonly BaiTap b = new("Buoi02_B8");

        [Theory(DisplayName = "B8 · Thứ (0 = Chủ nhật) – nhập ngày, tháng, năm")]
        [InlineData(13, 10, 2026, 2)]
        [InlineData(1, 1, 2000, 6)]
        [InlineData(2, 9, 1945, 0)]
        [InlineData(29, 2, 2024, 4)]
        public void Thu(int ngay, int thang, int nam, int mongDoi) =>
            KiemTra.SoCuoi(b.ChayMain($"{ngay}\n{thang}\n{nam}\n"), mongDoi, $"{ngay}/{thang}/{nam}");
    }
}
