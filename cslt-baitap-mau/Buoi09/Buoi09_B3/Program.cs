/*
 * Bài    : Buoi09_B3 – Xem từng byte của số nguyên (union)
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;
using System.Runtime.InteropServices;

namespace Buoi09_B3
{
    // Khai báo struct BonByte với [StructLayout(LayoutKind.Explicit)]:
    // int GiaTri ở offset 0 và bốn trường byte B0, B1, B2, B3 ở offset 0, 1, 2, 3.
    struct BonByte
    {
        public int GiaTri;
        public byte B0, B1, B2, B3;
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
    }
}
