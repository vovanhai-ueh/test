/*
 * Bài    : Buoi09_B2 – record SanPham, sắp xếp theo giá
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;

namespace Buoi09_B2
{
    record SanPham(string Ten, decimal Gia);

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Gọi thử các hàm của em ở đây, ví dụ:
            // Console.WriteLine(TenHam(...));
        }

        static void SapXepTheoGia(SanPham[] ds)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        static void SapXepTheoTen(SanPham[] ds)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        static SanPham GiamGia(SanPham sp, decimal phanTram)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }
    }
}
