/*
 * Bài    : Buoi09_B7 – Lớp MaTran có toán tử + và *
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;

namespace Buoi09_B7
{
    class MaTran
    {
        public int[,] DuLieu { get; }

        public MaTran(int[,] duLieu)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        public static MaTran operator +(MaTran x, MaTran y)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        public static MaTran operator *(MaTran x, MaTran y)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }
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
