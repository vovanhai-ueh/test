/*
 * Bài    : Buoi09_A3 – class PhanSo có đóng gói
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;

namespace Buoi09_A3
{
    class PhanSo
    {
        private int tu, mau;

        public PhanSo(int tu, int mau)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        public PhanSo Cong(PhanSo p)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        public PhanSo Nhan(PhanSo p)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        public double GiaTri
        {
            get { throw new NotImplementedException(); } // XÓA khi làm bài
        }

        public override string ToString()
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
