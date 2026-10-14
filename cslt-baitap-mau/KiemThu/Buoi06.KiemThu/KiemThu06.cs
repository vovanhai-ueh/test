using System.Linq;
using KiemThu.Chung;
using Xunit;

namespace KiemThu.Buoi06
{
    public class A1_ThongKeMang
    {
        readonly BaiTap b = new("Buoi06_A1");

        [Fact(DisplayName = "A1 · Tong, ViTriLonNhat, ViTriNhoNhat với {5, 3, 8, 1, 9}")]
        public void ThongKe()
        {
            int[] a = { 5, 3, 8, 1, 9 };
            KiemTra.Bang(26, b.Goi("Tong", a), "Tong");
            KiemTra.Bang(4, b.Goi("ViTriLonNhat", a), "ViTriLonNhat (phần tử ở cuối)");
            KiemTra.Bang(3, b.Goi("ViTriNhoNhat", a), "ViTriNhoNhat");
        }

        [Fact(DisplayName = "A1 · Mảng 1 phần tử, lớn nhất ở đầu")]
        public void Bien()
        {
            KiemTra.Bang(0, b.Goi("ViTriLonNhat", new[] { 7 }), "ViTriLonNhat({7})");
            KiemTra.Bang(0, b.Goi("ViTriLonNhat", new[] { 9, 1, 2 }), "ViTriLonNhat({9, 1, 2})");
            KiemTra.Bang(0, b.Goi("ViTriNhoNhat", new[] { -5, 1, 2 }), "ViTriNhoNhat({-5, 1, 2})");
            KiemTra.Bang(-3, b.Goi("Tong", new[] { -3 }), "Tong({-3})");
        }
    }

    public class A2_DemTimKiem
    {
        readonly BaiTap b = new("Buoi06_A2");
        readonly int[] a = { 4, 7, 2, 7, 9, 7 };

        [Fact(DisplayName = "A2 · DemSoChan")]
        public void DemSoChan() => KiemTra.Bang(2, b.Goi("DemSoChan", a), "DemSoChan({4, 7, 2, 7, 9, 7})");

        [Theory(DisplayName = "A2 · TimViTriCuoi")]
        [InlineData(7, 5)] [InlineData(5, -1)] [InlineData(4, 0)]
        public void ViTriCuoi(int x, int mongDoi) => KiemTra.Bang(mongDoi, b.Goi("TimViTriCuoi", a, x), $"TimViTriCuoi(a, {x})");

        [Fact(DisplayName = "A2 · InMoiViTri in 1 3 5, trả về 3")]
        public void MoiViTri()
        {
            KiemTra.Bang(3, b.Goi("InMoiViTri", a, 7), "InMoiViTri(a, 7) trả về");
            KiemTra.Bang("1 3 5", b.ManHinh.Trim(), "InMoiViTri(a, 7) in ra");
            KiemTra.Bang(0, b.Goi("InMoiViTri", a, 5), "InMoiViTri(a, 5) trả về");
        }
    }

    public class A3_DaoMang
    {
        readonly BaiTap b = new("Buoi06_A3");

        [Theory(DisplayName = "A3 · DaoMang")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 4, 3, 2, 1 })]
        [InlineData(new[] { 1, 2, 3 }, new[] { 3, 2, 1 })]
        [InlineData(new[] { 5 }, new[] { 5 })]
        public void Dao(int[] a, int[] mongDoi)
        {
            string truoc = BaiTap.BieuDien(a);
            b.Goi("DaoMang", a);
            KiemTra.Bang(mongDoi, a, $"Sau DaoMang({truoc})");
        }

        [Theory(DisplayName = "A3 · LaMangDoiXung và không làm đổi mảng")]
        [InlineData(new[] { 1, 2, 3, 2, 1 }, true)] [InlineData(new[] { 1, 2, 2, 1 }, true)]
        [InlineData(new[] { 1, 2, 3 }, false)] [InlineData(new[] { 5 }, true)]
        public void DoiXung(int[] a, bool mongDoi)
        {
            int[] goc = (int[])a.Clone();
            KiemTra.Bang(mongDoi, b.Goi("LaMangDoiXung", a), $"LaMangDoiXung({BaiTap.BieuDien(goc)})");
            KiemTra.Bang(goc, a, "Mảng sau khi gọi LaMangDoiXung phải giữ nguyên");
        }
    }

    public class A4_ChenXoa
    {
        readonly BaiTap b = new("Buoi06_A4");

        [Fact(DisplayName = "A4 · Kịch bản thêm 8, 9; chèn 5 vào 0, 3 vào 2; xóa vị trí 1")]
        public void KichBan()
        {
            int[] a = new int[100];
            object[] ts = { a, 0, 0, 8 };
            KiemTra.Bang(true, b.Goi("ChenPhanTu", ts), "ChenPhanTu thêm 8 vào mảng rỗng");
            ts = new object[] { a, ts[1], ts[1], 9 }; b.Goi("ChenPhanTu", ts);
            ts = new object[] { a, ts[1], 0, 5 }; b.Goi("ChenPhanTu", ts);
            ts = new object[] { a, ts[1], 2, 3 }; b.Goi("ChenPhanTu", ts);
            KiemTra.Bang(4, ts[1], "n sau 4 lần chèn");
            KiemTra.Bang(new[] { 5, 8, 3, 9 }, a.Take(4).ToArray(), "Mảng sau 4 lần chèn");
            object[] tx = { a, ts[1], 1 };
            KiemTra.Bang(true, b.Goi("XoaPhanTu", tx), "XoaPhanTu vị trí 1");
            KiemTra.Bang(3, tx[1], "n sau khi xóa");
            KiemTra.Bang(new[] { 5, 3, 9 }, a.Take(3).ToArray(), "Mảng sau khi xóa vị trí 1");
        }

        [Fact(DisplayName = "A4 · Từ chối vị trí sai và mảng đầy")]
        public void KhongHopLe()
        {
            int[] a = { 1, 2, 3 };
            KiemTra.Bang(false, b.Goi("ChenPhanTu", a, 3, 0, 9), "ChenPhanTu khi mảng đã đầy");
            KiemTra.Bang(false, b.Goi("ChenPhanTu", new int[5], 2, 3, 9), "ChenPhanTu vị trí 3 khi n = 2");
            KiemTra.Bang(false, b.Goi("XoaPhanTu", a, 3, 3), "XoaPhanTu vị trí 3 khi n = 3");
            KiemTra.Bang(false, b.Goi("XoaPhanTu", a, 0, 0), "XoaPhanTu khi mảng rỗng");
        }

        [Fact(DisplayName = "A4 · InMang chỉ in n phần tử đầu")]
        public void InMang()
        {
            b.Goi("InMang", new[] { 5, 3, 9, 0, 0 }, 3);
            KiemTra.Bang("5 3 9", b.ManHinh.Trim(), "InMang(a, 3)");
        }
    }

    public class A5_SapXepTimNhiPhan
    {
        readonly BaiTap b = new("Buoi06_A5");

        [Fact(DisplayName = "A5 · SapXepChon: kết quả và 15 phép so sánh")]
        public void SapXep()
        {
            int[] a = { 29, 3, 17, 8, 42, 11 };
            KiemTra.Bang(15, b.Goi("SapXepChon", a), "Số phép so sánh");
            KiemTra.Bang(new[] { 3, 8, 11, 17, 29, 42 }, a, "Mảng sau SapXepChon");
        }

        [Theory(DisplayName = "A5 · TimNhiPhan(a, x, out soBuoc)")]
        [InlineData(17, 3, 3)] [InlineData(42, 5, 3)] [InlineData(5, -1, 3)] [InlineData(3, 0, 2)]
        public void NhiPhan(int x, int viTri, int soBuoc)
        {
            object[] ts = { new[] { 3, 8, 11, 17, 29, 42 }, x, null };
            KiemTra.Bang(viTri, b.Goi("TimNhiPhan", ts), $"TimNhiPhan(a, {x})");
            KiemTra.Bang(soBuoc, ts[2], $"Số bước khi tìm {x}");
        }

        [Fact(DisplayName = "A5 · 1 000 000 phần tử: 20 bước")]
        public void MotTrieu()
        {
            int[] a = Enumerable.Range(0, 1_000_000).Select(i => 2 * i).ToArray();
            object[] ts = { a, 1999998, null };
            KiemTra.Bang(999999, b.Goi("TimNhiPhan", ts), "Vị trí của 1999998");
            KiemTra.Bang(20, ts[2], "Số bước");
        }
    }

    public class A6_ThongKeDiem
    {
        readonly BaiTap b = new("Buoi06_A6");
        readonly double[] d = { 7.5, 4, 9, 5, 6.5, 9 };

        [Fact(DisplayName = "A6 · TrungBinh và GiaTriLonNhat")]
        public void TrungBinh()
        {
            KiemTra.Gan(41.0 / 6, b.Goi<double>("TrungBinh", d), 1e-9, "TrungBinh");
            KiemTra.Bang(9.0, b.Goi("GiaTriLonNhat", d), "GiaTriLonNhat");
        }

        [Fact(DisplayName = "A6 · DemTheoLoai: Giỏi 2, Khá 2, TB 1, Yếu 1")]
        public void DemLoai()
        {
            object[] ts = { d, null, null, null, null };
            b.Goi("DemTheoLoai", ts);
            KiemTra.Bang(new[] { 2, 2, 1, 1 }, new[] { (int)ts[1], (int)ts[2], (int)ts[3], (int)ts[4] }, "DemTheoLoai (giỏi, khá, tb, yếu)");
        }
    }

    public class B1_NguyenToTrongMang
    {
        readonly BaiTap b = new("Buoi06_B1");

        [Fact(DisplayName = "B1 · InSoNguyenTo({2, 9, 11, 15, 17, 1})")]
        public void CoSo()
        {
            KiemTra.Bang(3, b.Goi("InSoNguyenTo", new[] { 2, 9, 11, 15, 17, 1 }), "Số lượng");
            KiemTra.Bang("2 11 17", b.ManHinh.Trim(), "Các số in ra");
        }

        [Fact(DisplayName = "B1 · Không có số nguyên tố")]
        public void KhongCo() => KiemTra.Bang(0, b.Goi("InSoNguyenTo", new[] { 4, 6, 8 }), "InSoNguyenTo({4, 6, 8})");
    }

    public class B2_LonThuHai
    {
        readonly BaiTap b = new("Buoi06_B2");

        [Theory(DisplayName = "B2 · TimLonThuHai(a, out ketQua)")]
        [InlineData(new[] { 5, 9, 3, 9, 7 }, true, 7)] [InlineData(new[] { 4, 4 }, false, 0)]
        [InlineData(new[] { 1, 2 }, true, 1)] [InlineData(new[] { 2, 1 }, true, 1)] [InlineData(new[] { -3, -8, -1 }, true, -3)]
        public void LonThuHai(int[] a, bool co, int giaTri)
        {
            object[] ts = { a, null };
            KiemTra.Bang(co, b.Goi("TimLonThuHai", ts), $"TimLonThuHai({BaiTap.BieuDien(a)})");
            if (co) KiemTra.Bang(giaTri, ts[1], $"Giá trị lớn thứ hai của {BaiTap.BieuDien(a)}");
        }
    }

    public class B3_TangDan
    {
        readonly BaiTap b = new("Buoi06_B3");

        [Theory(DisplayName = "B3 · LaTangDan")]
        [InlineData(new[] { 1, 3, 3, 5 }, true)] [InlineData(new[] { 1, 3, 2 }, false)] [InlineData(new[] { 7 }, true)] [InlineData(new[] { 2, 1 }, false)]
        public void TangDan(int[] a, bool mongDoi) => KiemTra.Bang(mongDoi, b.Goi("LaTangDan", a), $"LaTangDan({BaiTap.BieuDien(a)})");
    }

    public class B4_XoaMoiX
    {
        readonly BaiTap b = new("Buoi06_B4");

        [Theory(DisplayName = "B4 · XoaMoiX(a, ref n, x)")]
        [InlineData(3, new[] { 7, 5 })] [InlineData(9, new[] { 3, 7, 3, 3, 5, 3 })]
        public void Xoa(int x, int[] mongDoi)
        {
            int[] a = { 3, 7, 3, 3, 5, 3 };
            object[] ts = { a, 6, x };
            b.Goi("XoaMoiX", ts);
            KiemTra.Bang(mongDoi.Length, ts[1], $"n sau khi xóa {x}");
            KiemTra.Bang(mongDoi, a.Take((int)ts[1]).ToArray(), $"Mảng sau khi xóa {x}");
        }
    }

    public class B5_TronMang
    {
        readonly BaiTap b = new("Buoi06_B5");

        [Fact(DisplayName = "B5 · TronMang({1, 4, 7, 10}, {2, 3, 8})")]
        public void Tron() => KiemTra.Bang(new[] { 1, 2, 3, 4, 7, 8, 10 }, b.Goi("TronMang", new[] { 1, 4, 7, 10 }, new[] { 2, 3, 8 }), "TronMang");

        [Fact(DisplayName = "B5 · Một mảng rỗng")]
        public void Rong()
        {
            KiemTra.Bang(new[] { 1, 2 }, b.Goi("TronMang", new int[0], new[] { 1, 2 }), "TronMang({}, {1, 2})");
            KiemTra.Bang(new[] { 1, 2 }, b.Goi("TronMang", new[] { 1, 2 }, new int[0]), "TronMang({1, 2}, {})");
        }
    }

    public class B6_NoiBot
    {
        readonly BaiTap b = new("Buoi06_B6");

        [Fact(DisplayName = "B6 · SapXepNoiBot({5, 1, 4, 2, 8}): 4 lần đổi chỗ")]
        public void NoiBot()
        {
            int[] a = { 5, 1, 4, 2, 8 };
            KiemTra.Bang(4, b.Goi("SapXepNoiBot", a), "Số lần đổi chỗ");
            KiemTra.Bang(new[] { 1, 2, 4, 5, 8 }, a, "Mảng sau khi sắp xếp");
            KiemTra.Chua(b.ManHinh, "1 4 2 5 8", "Mảng in sau lượt 1");
        }
    }

    public class B7_DoanTang
    {
        readonly BaiTap b = new("Buoi06_B7");

        [Theory(DisplayName = "B7 · TimDoanTangDaiNhat(a, out batDau, out doDai)")]
        [InlineData(new[] { 1, 2, 5, 3, 4, 6, 8, 2 }, 3, 4)] [InlineData(new[] { 5, 4, 3 }, 0, 1)] [InlineData(new[] { 1, 2, 3 }, 0, 3)]
        public void DoanTang(int[] a, int batDau, int doDai)
        {
            object[] ts = { a, null, null };
            b.Goi("TimDoanTangDaiNhat", ts);
            KiemTra.Bang(new[] { batDau, doDai }, new[] { (int)ts[1], (int)ts[2] }, $"(bắt đầu, độ dài) của {BaiTap.BieuDien(a)}");
        }
    }

    public class B8_TanSuat
    {
        readonly BaiTap b = new("Buoi06_B8");

        [Fact(DisplayName = "B8 · DemTanSuat({4, 7, 2, 7, 9, 7, 4})")]
        public void TanSuat()
        {
            object[] ts = { new[] { 4, 7, 2, 7, 9, 7, 4 }, null, null };
            b.Goi("DemTanSuat", ts);
            KiemTra.Bang(new[] { 4, 7, 2, 9 }, ts[1], "giaTri (theo thứ tự xuất hiện lần đầu)");
            KiemTra.Bang(new[] { 2, 3, 1, 1 }, ts[2], "soLan");
        }
    }
}
