/*
 * Bài    : Buoi09_A6 – Quản lý sinh viên tổng hợp
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;

namespace Buoi09_A6
{
    class SinhVien
    {
        public string MSSV { get; }
        public string HoTen { get; set; }
        public double DiemTB { get; set; }

        public SinhVien(string mssv, string hoTen, double diemTB)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        public XepLoai Loai
        {
            get { throw new NotImplementedException(); } // XÓA khi làm bài
        }

        public string ToCsv()
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }
    }

    enum XepLoai { Yeu, TrungBinh, Kha, Gioi }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Gọi thử các hàm của em ở đây, ví dụ:
            // Console.WriteLine(TenHam(...));
        }

        static List<SinhVien> DocTep(string tep)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        static void GhiTep(string tep, List<SinhVien> ds)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }
    }
}
