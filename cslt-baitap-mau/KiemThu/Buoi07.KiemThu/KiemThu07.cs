using KiemThu.Chung;
using Xunit;

namespace KiemThu.Buoi07
{
    internal static class MT
    {
        public static readonly int[,] M23 = { { 1, -2, 3 }, { 4, 5, -6 } };
        public static readonly int[,] M33 = { { 3, 8, 1 }, { 9, 2, 9 }, { 4, 7, 5 } };
        public static int[,] M44()
        {
            var a = new int[4, 4]; int k = 1;
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) a[i, j] = k++;
            return a;
        }
    }

    public class A1_TongDongCot
    {
        readonly BaiTap b = new("Buoi07_A1");

        [Fact(DisplayName = "A1 · TongDong, TongCot với ma trận 2 × 3")]
        public void M23()
        {
            KiemTra.Bang(2, b.Goi("TongDong", MT.M23, 0), "TongDong(a, 0)");
            KiemTra.Bang(3, b.Goi("TongDong", MT.M23, 1), "TongDong(a, 1)");
            KiemTra.Bang(5, b.Goi("TongCot", MT.M23, 0), "TongCot(a, 0)");
            KiemTra.Bang(3, b.Goi("TongCot", MT.M23, 1), "TongCot(a, 1)");
            KiemTra.Bang(-3, b.Goi("TongCot", MT.M23, 2), "TongCot(a, 2)");
        }

        [Fact(DisplayName = "A1 · Ma trận 1 × 1")]
        public void M11()
        {
            KiemTra.Bang(7, b.Goi("TongDong", new[,] { { 7 } }, 0), "TongDong({{7}}, 0)");
            KiemTra.Bang(7, b.Goi("TongCot", new[,] { { 7 } }, 0), "TongCot({{7}}, 0)");
        }
    }

    public class A2_LonNhatNhoNhat
    {
        readonly BaiTap b = new("Buoi07_A2");

        [Fact(DisplayName = "A2 · TimLonNhat, TimNhoNhat, DemGiaTri")]
        public void ViTri()
        {
            object[] ts = { MT.M33, null, null };
            b.Goi("TimLonNhat", ts);
            KiemTra.Bang(new[] { 1, 0 }, new[] { (int)ts[1], (int)ts[2] }, "Vị trí lớn nhất (dòng, cột)");
            ts = new object[] { MT.M33, null, null };
            b.Goi("TimNhoNhat", ts);
            KiemTra.Bang(new[] { 0, 2 }, new[] { (int)ts[1], (int)ts[2] }, "Vị trí nhỏ nhất (dòng, cột)");
            KiemTra.Bang(2, b.Goi("DemGiaTri", MT.M33, 9), "DemGiaTri(a, 9)");
        }

        [Fact(DisplayName = "A2 · Lớn nhất ở góc dưới phải, ma trận không vuông")]
        public void Goc()
        {
            object[] ts = { new[,] { { 1, 2, 3 }, { 4, 5, 6 } }, null, null };
            b.Goi("TimLonNhat", ts);
            KiemTra.Bang(new[] { 1, 2 }, new[] { (int)ts[1], (int)ts[2] }, "Vị trí lớn nhất của {{1,2,3},{4,5,6}}");
        }
    }

    public class A3_DuongCheo
    {
        readonly BaiTap b = new("Buoi07_A3");

        [Fact(DisplayName = "A3 · Ma trận 4 × 4 chứa 1..16")]
        public void M44()
        {
            var a = MT.M44();
            KiemTra.Bang(34, b.Goi("TongCheoChinh", a), "TongCheoChinh");
            KiemTra.Bang(34, b.Goi("TongCheoPhu", a), "TongCheoPhu");
            KiemTra.Bang(36, b.Goi("TongTamGiacTren", a), "TongTamGiacTren (j > i)");
            KiemTra.Bang(66, b.Goi("TongTamGiacDuoi", a), "TongTamGiacDuoi (j < i)");
            KiemTra.Bang(false, b.Goi("LaTamGiacTren", a), "LaTamGiacTren");
        }

        [Fact(DisplayName = "A3 · LaTamGiacTren({{1, 2}, {0, 3}}) và ma trận 1 × 1")]
        public void TamGiacTren()
        {
            KiemTra.Bang(true, b.Goi("LaTamGiacTren", new[,] { { 1, 2 }, { 0, 3 } }), "LaTamGiacTren({{1, 2}, {0, 3}})");
            KiemTra.Bang(5, b.Goi("TongCheoPhu", new[,] { { 5 } }), "TongCheoPhu({{5}})");
        }
    }

    public class A4_PhepToanMaTran
    {
        readonly BaiTap b = new("Buoi07_A4");
        static readonly int[,] A = { { 1, 2 }, { 3, 4 } }, B = { { 5, 6 }, { 7, 8 } };

        [Fact(DisplayName = "A4 · CongMaTran")]
        public void Cong()
        {
            KiemTra.Bang(new[,] { { 6, 8 }, { 10, 12 } }, b.Goi("CongMaTran", A, B), "A + B");
            KiemTra.Bang(null, b.Goi("CongMaTran", A, new int[2, 3]), "Cộng ma trận khác kích thước phải trả về null");
        }

        [Fact(DisplayName = "A4 · NhanMaTran")]
        public void Nhan()
        {
            KiemTra.Bang(new[,] { { 19, 22 }, { 43, 50 } }, b.Goi("NhanMaTran", A, B), "A × B");
            KiemTra.Bang(new[,] { { 58, 64 }, { 139, 154 } },
                b.Goi("NhanMaTran", new[,] { { 1, 2, 3 }, { 4, 5, 6 } }, new[,] { { 7, 8 }, { 9, 10 }, { 11, 12 } }), "(2 × 3) × (3 × 2)");
            KiemTra.Bang(null, b.Goi("NhanMaTran", A, new int[3, 2]), "Nhân khi số cột A ≠ số dòng B phải trả về null");
        }

        [Fact(DisplayName = "A4 · ChuyenVi ma trận 2 × 3")]
        public void ChuyenVi() =>
            KiemTra.Bang(new[,] { { 1, 4 }, { 2, 5 }, { 3, 6 } }, b.Goi("ChuyenVi", new[,] { { 1, 2, 3 }, { 4, 5, 6 } }), "ChuyenVi");
    }

    public class A5_Pascal
    {
        readonly BaiTap b = new("Buoi07_A5");

        [Fact(DisplayName = "A5 · TaoPascal(6)")]
        public void Pascal()
        {
            var p = (int[][])b.Goi("TaoPascal", 6);
            KiemTra.Bang(6, p.Length, "Số dòng");
            KiemTra.Bang(new[] { 1, 5, 10, 10, 5, 1 }, p[5], "Dòng cuối");
            for (int i = 0; i < 6; i++) KiemTra.Bang(i + 1, p[i].Length, $"Độ dài dòng {i} (mảng răng cưa)");
        }

        [Fact(DisplayName = "A5 · TaoPascal(1)")]
        public void MotDong() => KiemTra.Bang(new[] { 1 }, ((int[][])b.Goi("TaoPascal", 1))[0], "TaoPascal(1)[0]");
    }

    public class A6_BangDiem
    {
        readonly BaiTap b = new("Buoi07_A6");
        static readonly double[,] D = { { 8, 7.5, 9 }, { 6, 5.5, 7 }, { 9, 8, 10 }, { 4, 6, 5.5 } };

        [Fact(DisplayName = "A6 · TrungBinhDong, TrungBinhCot")]
        public void TrungBinh()
        {
            KiemTra.Gan(24.5 / 3, b.Goi<double>("TrungBinhDong", D, 0), 1e-9, "TrungBinhDong(d, 0)");
            KiemTra.Gan(9, b.Goi<double>("TrungBinhDong", D, 2), 1e-9, "TrungBinhDong(d, 2)");
            KiemTra.Gan(6.75, b.Goi<double>("TrungBinhCot", D, 0), 1e-9, "TrungBinhCot(d, 0) – Toán");
            KiemTra.Gan(7.875, b.Goi<double>("TrungBinhCot", D, 2), 1e-9, "TrungBinhCot(d, 2) – Tin");
        }
    }

    public class B1_AmDuong
    {
        readonly BaiTap b = new("Buoi07_B1");

        [Theory(DisplayName = "B1 · DemAmDuongDong")]
        [InlineData(0, 2, 1)] [InlineData(1, 1, 2)] [InlineData(2, 0, 2)]
        public void AmDuong(int dong, int am, int duong)
        {
            object[] ts = { new[,] { { -1, 2, -3 }, { 4, -5, 6 }, { 0, 1, 2 } }, dong, null, null };
            b.Goi("DemAmDuongDong", ts);
            KiemTra.Bang(new[] { am, duong }, new[] { (int)ts[2], (int)ts[3] }, $"Dòng {dong} (số âm, số dương)");
        }
    }

    public class B2_DonViDoiXung
    {
        readonly BaiTap b = new("Buoi07_B2");

        [Fact(DisplayName = "B2 · TaoMaTranDonVi(3)")]
        public void DonVi() => KiemTra.Bang(new[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }, b.Goi("TaoMaTranDonVi", 3), "TaoMaTranDonVi(3)");

        [Fact(DisplayName = "B2 · LaDoiXung")]
        public void DoiXung()
        {
            KiemTra.Bang(true, b.Goi("LaDoiXung", new[,] { { 1, 7 }, { 7, 2 } }), "{{1, 7}, {7, 2}}");
            KiemTra.Bang(false, b.Goi("LaDoiXung", new[,] { { 1, 2 }, { 3, 4 } }), "{{1, 2}, {3, 4}}");
            KiemTra.Bang(true, b.Goi("LaDoiXung", new[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }), "Ma trận đơn vị 3 × 3");
        }
    }

    public class B3_TongLonNhat
    {
        readonly BaiTap b = new("Buoi07_B3");
        static readonly int[,] A = { { 3, 1, 2 }, { 8, 0, 1 }, { 2, 9, 1 } };

        [Fact(DisplayName = "B3 · DongTongLonNhat, CotTongLonNhat")]
        public void LonNhat()
        {
            KiemTra.Bang(2, b.Goi("DongTongLonNhat", A), "DongTongLonNhat");
            KiemTra.Bang(0, b.Goi("CotTongLonNhat", A), "CotTongLonNhat (cột 0 tổng 13)");
        }
    }

    public class B4_Xoay90
    {
        readonly BaiTap b = new("Buoi07_B4");

        [Fact(DisplayName = "B4 · XoayPhai90 ma trận 2 × 3")]
        public void Xoay() =>
            KiemTra.Bang(new[,] { { 4, 1 }, { 5, 2 }, { 6, 3 } }, b.Goi("XoayPhai90", new[,] { { 1, 2, 3 }, { 4, 5, 6 } }), "XoayPhai90");

        [Fact(DisplayName = "B4 · Xoay 4 lần về ma trận ban đầu")]
        public void BonLan()
        {
            object a = MT.M23;
            for (int k = 0; k < 4; k++) a = b.Goi("XoayPhai90", a);
            KiemTra.Bang(MT.M23, a, "Sau 4 lần xoay");
        }
    }

    public class B5_YenNgua
    {
        readonly BaiTap b = new("Buoi07_B5");

        [Fact(DisplayName = "B5 · Có điểm yên ngựa 7 tại (2, 0)")]
        public void Co()
        {
            KiemTra.Bang(1, b.Goi("InDiemYenNgua", new[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }), "Số điểm yên ngựa");
            KiemTra.Chua(b.ManHinh, "7", "Màn hình in giá trị 7");
        }

        [Fact(DisplayName = "B5 · Không có điểm yên ngựa")]
        public void KhongCo()
        {
            KiemTra.Bang(0, b.Goi("InDiemYenNgua", MT.M33), "Số điểm yên ngựa");
            KiemTra.ChuaGanDung(b.ManHinh, "Không có", "Thông báo");
        }
    }

    public class B6_RangCua
    {
        readonly BaiTap b = new("Buoi07_B6");

        [Fact(DisplayName = "B6 · SapXepTungDong rồi SapXepTheoDoDai")]
        public void SapXep()
        {
            int[][] a = { new[] { 5, 1, 4 }, new[] { 9 }, new[] { 3, 8, 2, 7 } };
            b.Goi("SapXepTungDong", (object)a);
            KiemTra.Bang(new[] { 1, 4, 5 }, a[0], "Dòng 0");
            KiemTra.Bang(new[] { 2, 3, 7, 8 }, a[2], "Dòng 2");
            int[] dong2 = a[2];
            b.Goi("SapXepTheoDoDai", (object)a);
            KiemTra.Bang(new[] { 1, 3, 4 }, new[] { a[0].Length, a[1].Length, a[2].Length }, "Độ dài các dòng sau SapXepTheoDoDai");
            KiemTra.Dung(ReferenceEquals(a[2], dong2), "SapXepTheoDoDai phải đổi chỗ tham chiếu a[i], a[j] chứ không chép phần tử.");
        }
    }

    public class B7_XoanOc
    {
        readonly BaiTap b = new("Buoi07_B7");

        [Fact(DisplayName = "B7 · TaoXoanOc(4)")]
        public void N4() => KiemTra.Bang(new[,] { { 1, 2, 3, 4 }, { 12, 13, 14, 5 }, { 11, 16, 15, 6 }, { 10, 9, 8, 7 } }, b.Goi("TaoXoanOc", 4), "TaoXoanOc(4)");

        [Fact(DisplayName = "B7 · TaoXoanOc(1) và TaoXoanOc(3)")]
        public void N13()
        {
            KiemTra.Bang(new[,] { { 1 } }, b.Goi("TaoXoanOc", 1), "TaoXoanOc(1)");
            KiemTra.Bang(new[,] { { 1, 2, 3 }, { 8, 9, 4 }, { 7, 6, 5 } }, b.Goi("TaoXoanOc", 3), "TaoXoanOc(3)");
        }
    }

    public class B8_DoMin
    {
        readonly BaiTap b = new("Buoi07_B8");

        [Fact(DisplayName = "B8 · DoMin bản đồ 4 × 4")]
        public void DoMin() =>
            KiemTra.Bang(new[] { "*100", "2210", "1*10", "1110" }, b.Goi("DoMin", (object)new[] { "*...", "....", ".*..", "...." }), "DoMin");

        [Fact(DisplayName = "B8 · Bản đồ 1 × 1 và toàn mìn")]
        public void Bien()
        {
            KiemTra.Bang(new[] { "0" }, b.Goi("DoMin", (object)new[] { "." }), "DoMin({\".\"})");
            KiemTra.Bang(new[] { "**", "**" }, b.Goi("DoMin", (object)new[] { "**", "**" }), "DoMin toàn mìn");
        }
    }
}
