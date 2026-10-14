using KiemThu.Chung;
using Xunit;

// Buổi 1: kiểm tra theo màn hình. Bài A6, B4–B8 làm trên giấy, không chấm tự động.
namespace KiemThu.Buoi01
{
    public class A1_ChuongTrinhDauTien
    {
        readonly BaiTap b = new("Buoi01_A1");

        [Fact(DisplayName = "A1 · In dòng chào")]
        public void InDongChao() => KiemTra.Chua(b.ChayMain(), "Xin chào, C#!", "Chương trình chào");
    }

    public class A2_TheSinhVien
    {
        readonly BaiTap b = new("Buoi01_A2");

        [Fact(DisplayName = "A2 · Có tiêu đề và các mục của thẻ")]
        public void TheSinhVien()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "THẺ SINH VIÊN", "Tiêu đề thẻ");
            foreach (string muc in new[] { "Họ tên", "MSSV", "Lớp" })
                KiemTra.Chua(ra, muc, $"Mục \"{muc}\"");
            KiemTra.Chua(ra, "======", "Khung thẻ");
        }
    }

    public class A3_VeHinh
    {
        readonly BaiTap b = new("Buoi01_A3");

        [Fact(DisplayName = "A3 · Ngôi nhà bằng ký tự (có dấu \\)")]
        public void NgoiNha()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "/\\", "Mái nhà");
            KiemTra.Chua(ra, "/____\\", "Chân mái");
            KiemTra.Chua(ra, "|_[]_|", "Cửa");
        }
    }

    public class A4_SanLoi
    {
        readonly BaiTap b = new("Buoi01_A4");

        [Fact(DisplayName = "A4 · Chương trình đã sửa chạy được")]
        public void ChayDuoc()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "Săn lỗi C#", "Dòng đầu");
            KiemTra.Chua(ra, "Xong!", "Dòng cuối");
        }
    }

    public class A5_HinhChuNhat
    {
        readonly BaiTap b = new("Buoi01_A5");

        [Fact(DisplayName = "A5 · Chu vi 16, diện tích 15 với a = 5, b = 3")]
        public void ChuViDienTich()
        {
            string ra = b.ChayMain();
            KiemTra.ChuaGanDung(ra, "Chu vi", "Nhãn chu vi");
            KiemTra.Dung(KiemTra.CacSo(ra).Contains(16), "Chu vi phải là 16.\nMàn hình:\n" + ra);
            KiemTra.Dung(KiemTra.CacSo(ra).Contains(15), "Diện tích phải là 15.\nMàn hình:\n" + ra);
        }
    }

    public class B1_ThoiKhoaBieu
    {
        readonly BaiTap b = new("Buoi01_B1");

        [Fact(DisplayName = "B1 · Có tiêu đề và dùng ký tự tab")]
        public void ThoiKhoaBieu()
        {
            string ra = b.ChayMain();
            KiemTra.ChuaGanDung(ra, "THỜI KHÓA BIỂU", "Tiêu đề");
            KiemTra.Chua(ra, "\t", "Ký tự \\t để thẳng cột");
        }
    }

    public class B2_Thiep
    {
        readonly BaiTap b = new("Buoi01_B2");

        [Fact(DisplayName = "B2 · Thiệp có khung dấu *")]
        public void Thiep()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "*****", "Khung dấu *");
            KiemTra.Dung(ra.Split('\n').Length >= 3, "Thiệp phải có ít nhất 3 dòng.");
        }
    }

    public class B3_SuaLoi
    {
        readonly BaiTap b = new("Buoi01_B3");

        [Fact(DisplayName = "B3 · Kết quả sau khi sửa đủ lỗi")]
        public void KetQua()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "Chương trình có lỗi", "Dòng 1");
            KiemTra.Chua(ra, @"C:\Temp\new", "Đường dẫn giữ nguyên dấu \\ (dùng @\"...\" hoặc \\\\)");
            KiemTra.Chua(ra, "Kết thúc", "Dòng cuối");
        }
    }
}
