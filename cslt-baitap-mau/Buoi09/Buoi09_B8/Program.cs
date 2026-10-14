/*
 * Bài    : Buoi09_B8 – Hàng đợi vòng generic
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;

namespace Buoi09_B8
{
    class HangDoiVong<T>
    {
        private T[] a;
        private int dau, soPhanTu;

        public HangDoiVong(int sucChua)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        public int SoPhanTu => soPhanTu;

        public bool Them(T x)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        public T Lay()
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
