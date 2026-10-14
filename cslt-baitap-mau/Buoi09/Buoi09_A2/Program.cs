/*
 * Bài    : Buoi09_A2 – enum và switch
 * Họ tên : [điền họ tên]
 * MSSV   : [điền MSSV]
 */
using System.Globalization;

namespace Buoi09_A2
{
    enum XepLoai { Yeu, TrungBinh, Kha, Gioi }
    enum Thu { ChuNhat, ThuHai, ThuBa, ThuTu, ThuNam, ThuSau, ThuBay }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            // Gọi thử các hàm của em ở đây, ví dụ:
            // Console.WriteLine(TenHam(...));
        }

        static XepLoai PhanLoai(double tb)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        static Thu ThuTrongTuan(int ngay, int thang, int nam)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }

        static string TenTiengViet(Thu t)
        {
            throw new NotImplementedException(); // XÓA dòng này khi làm bài
        }
    }
}
