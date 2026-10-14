/*
 * Bài    : Buoi09_A5 – Hàm generic và lambda
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;

namespace Buoi09_A5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Gọi thử các hàm của em ở đây, ví dụ:
            // Console.WriteLine(TenHam(...));
        }

        static T TimLonNhat<T>(T[] a) where T : IComparable<T>
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        static int DemThoa<T>(T[] a, Predicate<T> dieuKien)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }
    }
}
