/*
 * Bài    : Buoi09_A1 – struct SinhVien thay cho mảng song song
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;

namespace Buoi09_A1
{
    struct SinhVien
    {
        public string MSSV;
        public string HoTen;
        public double DiemTB;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Gọi thử các hàm của em ở đây, ví dụ:
            // Console.WriteLine(TenHam(...));
        }

        static void SapXepGiamTheoDiem(SinhVien[] ds)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }
    }
}
