using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using KiemThu.Chung;
using Xunit;

namespace KiemThu.Buoi09
{
    internal static class SV
    {
        /// <summary>Tạo mảng struct/class SinhVien của bài làm từ bộ (mssv, họ tên, điểm).</summary>
        public static Array TaoMang(BaiTap b, params (string ma, string ten, double diem)[] ds)
        {
            Type t = b.Kieu("SinhVien");
            Array a = Array.CreateInstance(t, ds.Length);
            for (int i = 0; i < ds.Length; i++)
            {
                object sv = b.TaoTuKieu(t);
                sv = b.Gan(sv, "MSSV", ds[i].ma);
                sv = b.Gan(sv, "HoTen", ds[i].ten);
                sv = b.Gan(sv, "DiemTB", ds[i].diem);
                a.SetValue(sv, i);
            }
            return a;
        }

        public static string[] Ma(BaiTap b, IEnumerable ds) => ds.Cast<object>().Select(x => (string)b.Lay(x, "MSSV")).ToArray();
    }

    public class A1_StructSinhVien
    {
        readonly BaiTap b = new("Buoi09_A1");

        [Fact(DisplayName = "A1 · struct SinhVien có MSSV, HoTen, DiemTB")]
        public void KhaiBao()
        {
            Type t = b.Kieu("SinhVien");
            KiemTra.Dung(t.IsValueType, "SinhVien phải là struct (kiểu giá trị).");
        }

        [Fact(DisplayName = "A1 · SapXepGiamTheoDiem")]
        public void SapXep()
        {
            Array ds = SV.TaoMang(b, ("SV01", "An", 6.8), ("SV02", "Bình", 9.1), ("SV03", "Chi", 4.5), ("SV04", "Dũng", 8));
            b.Goi("SapXepGiamTheoDiem", ds);
            KiemTra.Bang(new[] { "SV02", "SV04", "SV01", "SV03" }, SV.Ma(b, ds), "Thứ tự MSSV sau khi sắp xếp giảm theo điểm");
        }
    }

    public class A2_Enum
    {
        readonly BaiTap b = new("Buoi09_A2");

        [Fact(DisplayName = "A2 · Khai báo enum XepLoai và Thu đúng thứ tự")]
        public void KhaiBao()
        {
            KiemTra.Bang(new[] { "Yeu", "TrungBinh", "Kha", "Gioi" }, Enum.GetNames(b.Kieu("XepLoai")), "Các giá trị của XepLoai");
            KiemTra.Bang(new[] { "ChuNhat", "ThuHai", "ThuBa", "ThuTu", "ThuNam", "ThuSau", "ThuBay" }, Enum.GetNames(b.Kieu("Thu")), "Các giá trị của Thu");
        }

        [Theory(DisplayName = "A2 · PhanLoai trả về enum")]
        [InlineData(9.1, "Gioi")] [InlineData(8, "Gioi")] [InlineData(6.8, "Kha")] [InlineData(5, "TrungBinh")] [InlineData(4.5, "Yeu")]
        public void PhanLoai(double tb, string mongDoi) => KiemTra.Bang(mongDoi, b.Goi("PhanLoai", tb)?.ToString(), $"PhanLoai({tb})");

        [Theory(DisplayName = "A2 · ThuTrongTuan trả về enum Thu")]
        [InlineData(13, 10, 2026, "ThuBa")] [InlineData(2, 9, 1945, "ChuNhat")] [InlineData(1, 1, 2000, "ThuBay")]
        public void Thu(int d, int m, int y, string mongDoi) => KiemTra.Bang(mongDoi, b.Goi("ThuTrongTuan", d, m, y)?.ToString(), $"ThuTrongTuan({d}, {m}, {y})");

        [Fact(DisplayName = "A2 · TenTiengViet")]
        public void TenTiengViet()
        {
            Type t = b.Kieu("Thu");
            KiemTra.ChuoiBang("Chủ nhật", b.Goi<string>("TenTiengViet", Enum.Parse(t, "ChuNhat")), "TenTiengViet(Thu.ChuNhat)");
            KiemTra.ChuoiBang("Thứ Ba", b.Goi<string>("TenTiengViet", Enum.Parse(t, "ThuBa")), "TenTiengViet(Thu.ThuBa)");
            KiemTra.ChuoiBang("Thứ Bảy", b.Goi<string>("TenTiengViet", Enum.Parse(t, "ThuBay")), "TenTiengViet(Thu.ThuBay)");
        }
    }

    public class A3_PhanSo
    {
        readonly BaiTap b = new("Buoi09_A3");
        string PS(int t, int m) => b.Tao("PhanSo", t, m).ToString();

        [Theory(DisplayName = "A3 · Hàm tạo rút gọn, mẫu dương, ToString")]
        [InlineData(2, 4, "1/2")] [InlineData(3, -6, "-1/2")] [InlineData(4, 2, "2")] [InlineData(0, 5, "0")] [InlineData(-6, -8, "3/4")]
        public void HamTao(int t, int m, string mongDoi)
        {
            object p = b.Tao("PhanSo", t, m);
            KiemTra.Bang(mongDoi, b.ChayCoGioiHan(() => p.ToString(), null, $"new PhanSo({t}, {m}).ToString()"), $"new PhanSo({t}, {m})");
        }

        [Fact(DisplayName = "A3 · Cong, Nhan, GiaTri")]
        public void PhepToan()
        {
            object a = b.Tao("PhanSo", 1, 2), c = b.Tao("PhanSo", 1, 3);
            KiemTra.Bang("5/6", b.GoiPhuongThucDoiTuong(a, "Cong", c)?.ToString(), "1/2 + 1/3");
            KiemTra.Bang("1/6", b.GoiPhuongThucDoiTuong(a, "Nhan", c)?.ToString(), "1/2 × 1/3");
            KiemTra.Gan(0.5, Convert.ToDouble(b.Lay(a, "GiaTri")), 1e-12, "GiaTri của 1/2");
        }

        [Fact(DisplayName = "A3 · Mẫu số 0 ném ArgumentException")]
        public void MauKhong()
        {
            ConstructorInfo ctor = b.Kieu("PhanSo").GetConstructor(new[] { typeof(int), typeof(int) });
            KiemTra.Dung(ctor != null, "Thiếu hàm tạo PhanSo(int tu, int mau).");
            b.PhaiNem<ArgumentException>(() => ctor.Invoke(new object[] { 1, 0 }), "new PhanSo(1, 0)");
        }

        [Fact(DisplayName = "A3 · Trường tu, mau là private")]
        public void DongGoi()
        {
            var f = b.Kieu("PhanSo").GetFields(BindingFlags.Instance | BindingFlags.Public);
            KiemTra.Dung(f.Length == 0, "PhanSo không được có trường public (đóng gói).");
        }
    }

    public class A4_ListDictionary
    {
        readonly BaiTap b = new("Buoi09_A4");

        [Fact(DisplayName = "A4 · Kết quả List và Dictionary")]
        public void KetQua()
        {
            string ra = b.ChayMain();
            KiemTra.Chua(ra, "9 3 8 1 4 21", "Dòng List");
            KiemTra.Chua(ra, "2 7 False", "Dòng Dictionary");
            KiemTra.Chua(ra, "SV01=7 SV02=9.1", "Duyệt Dictionary");
        }
    }

    public class A5_Generic
    {
        readonly BaiTap b = new("Buoi09_A5");

        [Fact(DisplayName = "A5 · TimLonNhat<T> với int, string, double")]
        public void TimLonNhat()
        {
            KiemTra.Bang(9, b.GoiGeneric("TimLonNhat", new[] { typeof(int) }, new[] { 3, 9, 2 }), "TimLonNhat({3, 9, 2})");
            KiemTra.Bang("lê", b.GoiGeneric("TimLonNhat", new[] { typeof(string) }, (object)new[] { "lê", "an", "bình" }), "TimLonNhat({\"lê\", \"an\", \"bình\"})");
            KiemTra.Bang(2.5, b.GoiGeneric("TimLonNhat", new[] { typeof(double) }, new[] { 2.5, -1.0 }), "TimLonNhat({2.5, -1.0})");
        }

        [Fact(DisplayName = "A5 · DemThoa<T> với lambda")]
        public void DemThoa()
        {
            int[] so = { 2, 9, 11, 15, 17, 1, 4 };
            KiemTra.Bang(2, b.GoiGeneric("DemThoa", new[] { typeof(int) }, so, new Predicate<int>(x => x % 2 == 0)), "DemThoa(so, x => x % 2 == 0)");
            KiemTra.Bang(3, b.GoiGeneric("DemThoa", new[] { typeof(int) }, so, new Predicate<int>(x => x > 10)), "DemThoa(so, x => x > 10)");
            KiemTra.Bang(2, b.GoiGeneric("DemThoa", new[] { typeof(string) }, new[] { "An", "Bình", "Anh" }, new Predicate<string>(s => s.StartsWith("A"))),
                "DemThoa(ten, s => s.StartsWith(\"A\"))");
        }
    }

    public class A6_QuanLySinhVien
    {
        readonly BaiTap b = new("Buoi09_A6");

        [Fact(DisplayName = "A6 · class SinhVien: hàm tạo, Loai, ToCsv")]
        public void LopSinhVien()
        {
            object sv = b.Tao("SinhVien", "SV01", "Nguyễn An", 6.8);
            KiemTra.Bang("SV01", b.Lay(sv, "MSSV"), "MSSV");
            KiemTra.Bang("Kha", b.Lay(sv, "Loai")?.ToString(), "Loai của điểm 6.8");
            KiemTra.Bang("SV01,Nguyễn An,6.8", b.GoiPhuongThucDoiTuong(sv, "ToCsv"), "ToCsv()");
            KiemTra.Bang("Gioi", b.Lay(b.Tao("SinhVien", "SV02", "Trần Bình", 9.1), "Loai")?.ToString(), "Loai của điểm 9.1");
        }

        [Fact(DisplayName = "A6 · GhiTep rồi DocTep, bỏ dòng sai")]
        public void Tep()
        {
            string tep = Path.Combine(BaiTap.ThuMucTam(), "sinhvien.csv");
            Type tSV = b.Kieu("SinhVien");
            var ds = (IList)Activator.CreateInstance(typeof(System.Collections.Generic.List<>).MakeGenericType(tSV));
            ds.Add(b.Tao("SinhVien", "SV01", "Nguyễn An", 6.8));
            ds.Add(b.Tao("SinhVien", "SV02", "Trần Bình", 9.1));
            b.Goi("GhiTep", tep, ds);
            File.AppendAllLines(tep, new[] { "dòng hỏng", "SV09,X,abc" });
            var doc = (IList)b.Goi("DocTep", tep);
            KiemTra.Bang(new[] { "SV01", "SV02" }, SV.Ma(b, doc), "MSSV đọc lại (bỏ 2 dòng sai)");
            KiemTra.Bang(0, ((IList)b.Goi("DocTep", tep + ".khong")).Count, "DocTep khi tệp chưa có");
        }
    }

    public class B1_Diem2D
    {
        readonly BaiTap b = new("Buoi09_B1");

        [Fact(DisplayName = "B1 · KhoangCach (0,0)–(3,4) = 5")]
        public void KhoangCach()
        {
            object p = b.Tao("Diem2D", 0.0, 0.0), q = b.Tao("Diem2D", 3.0, 4.0);
            KiemTra.Gan(5, Convert.ToDouble(b.GoiPhuongThucDoiTuong(p, "KhoangCach", q)), 1e-9, "p.KhoangCach(q)");
            KiemTra.Dung(b.Kieu("Diem2D").IsValueType, "Diem2D phải là struct.");
        }

        [Fact(DisplayName = "B1 · ChuViDaGiac hình vuông cạnh 1")]
        public void ChuVi()
        {
            Type t = b.Kieu("Diem2D");
            Array ds = Array.CreateInstance(t, 4);
            double[,] xy = { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 0, 1 } };
            for (int i = 0; i < 4; i++) ds.SetValue(b.TaoTuKieu(t, xy[i, 0], xy[i, 1]), i);
            KiemTra.Gan(4, b.Goi<double>("ChuViDaGiac", ds), 1e-9, "ChuViDaGiac");
        }
    }

    public class B2_Record
    {
        readonly BaiTap b = new("Buoi09_B2");
        Array Mang()
        {
            Type t = b.Kieu("SanPham");
            Array a = Array.CreateInstance(t, 3);
            a.SetValue(b.Tao("SanPham", "Vở", 12000m), 0);
            a.SetValue(b.Tao("SanPham", "Bút", 5000m), 1);
            a.SetValue(b.Tao("SanPham", "Thước", 8000m), 2);
            return a;
        }
        string[] Ten(Array a) => a.Cast<object>().Select(x => (string)b.Lay(x, "Ten")).ToArray();

        [Fact(DisplayName = "B2 · SapXepTheoGia")]
        public void TheoGia()
        {
            Array a = Mang();
            b.Goi("SapXepTheoGia", a);
            KiemTra.Bang(new[] { "Bút", "Thước", "Vở" }, Ten(a), "Thứ tự sau SapXepTheoGia");
            KiemTra.Bang("SanPham { Ten = Bút, Gia = 5000 }", a.GetValue(0).ToString(), "ToString() của record");
        }

        [Fact(DisplayName = "B2 · SapXepTheoTen")]
        public void TheoTen()
        {
            Array a = Mang();
            b.Goi("SapXepTheoTen", a);
            KiemTra.Bang(new[] { "Bút", "Thước", "Vở" }, Ten(a), "Thứ tự sau SapXepTheoTen");
        }

        [Fact(DisplayName = "B2 · So sánh == và GiamGia bằng with")]
        public void BangNhauVaWith()
        {
            object x = b.Tao("SanPham", "Bút", 5000m), y = b.Tao("SanPham", "Bút", 5000m);
            KiemTra.Dung(x.Equals(y), "Hai record cùng nội dung phải bằng nhau (record, không phải class).");
            object g = b.Goi("GiamGia", x, 10m);
            KiemTra.Bang(4500m, b.Lay(g, "Gia"), "GiamGia(Bút 5000, 10%)");
            KiemTra.Bang(5000m, b.Lay(x, "Gia"), "Bản gốc không đổi sau with");
        }
    }

    public class B3_Union
    {
        readonly BaiTap b = new("Buoi09_B3");

        object Dat(int v)
        {
            Type t = b.Kieu("BonByte");
            var sl = t.StructLayoutAttribute;
            if (sl == null || sl.Value != LayoutKind.Explicit)
                KiemTra.Loi("[CHƯA LÀM] BonByte chưa khai báo [StructLayout(LayoutKind.Explicit)] với [FieldOffset] cho từng trường.");
            return b.Gan(b.TaoTuKieu(t), "GiaTri", v);
        }
        int[] Bon(object o) => new[] { "B0", "B1", "B2", "B3" }.Select(n => Convert.ToInt32(b.Lay(o, n))).ToArray();

        [Theory(DisplayName = "B3 · Bốn byte của số nguyên (little-endian)")]
        [InlineData(0x01020304, 4, 3, 2, 1)] [InlineData(258, 2, 1, 0, 0)] [InlineData(-1, 255, 255, 255, 255)]
        public void BonByte(int v, int b0, int b1, int b2, int b3) => KiemTra.Bang(new[] { b0, b1, b2, b3 }, Bon(Dat(v)), $"B0..B3 khi GiaTri = {v}");
    }

    public unsafe class B4_ConTro
    {
        readonly BaiTap b = new("Buoi09_B4");

        [Theory(DisplayName = "B4 · DaoMang(int* p, int n)")]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 5, 4, 3, 2, 1 })] [InlineData(new[] { 7 }, new[] { 7 })] [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
        public void Dao(int[] a, int[] mongDoi)
        {
            string truoc = BaiTap.BieuDien(a);
            fixed (int* p = a)
            {
                object con = Pointer.Box(p, typeof(int*));
                b.Goi("DaoMang", con, a.Length);
            }
            KiemTra.Bang(mongDoi, a, $"DaoMang({truoc})");
        }
    }

    public class B5_Cap
    {
        readonly BaiTap b = new("Buoi09_B5");

        [Fact(DisplayName = "B5 · Cap<string, int> và Dao()")]
        public void Cap()
        {
            Type t = b.Kieu("Cap").MakeGenericType(typeof(string), typeof(int));
            object c = b.TaoTuKieu(t, "An", 8);
            KiemTra.Bang("(An, 8)", b.ChayCoGioiHan(() => c.ToString(), null, "Cap.ToString()"), "new Cap<string, int>(\"An\", 8)");
            object d = b.GoiPhuongThucDoiTuong(c, "Dao");
            KiemTra.Bang("(8, An)", d?.ToString(), "Dao()");
            KiemTra.Bang(8, b.Lay(d, "Dau"), "Dao().Dau");
        }

        [Fact(DisplayName = "B5 · TimMinMax trả về Cap<int, int>")]
        public void MinMax()
        {
            object c = b.Goi("TimMinMax", new[] { 4, -2, 9, 0 });
            KiemTra.Bang(new[] { -2, 9 }, new[] { (int)b.Lay(c, "Dau"), (int)b.Lay(c, "Sau") }, "(min, max) của {4, -2, 9, 0}");
        }
    }

    public class B6_Lambda
    {
        readonly BaiTap b = new("Buoi09_B6");
        Array DS() => SV.TaoMang(b, ("SV01", "Chi", 6.8), ("SV02", "An", 9.1), ("SV03", "Bình", 4.5), ("SV04", "Dũng", 8));

        [Fact(DisplayName = "B6 · LocGioi, TimDauTienDuoi5, CoAiDat10")]
        public void Loc()
        {
            KiemTra.Bang(new[] { "SV02", "SV04" }, SV.Ma(b, (IEnumerable)b.Goi("LocGioi", DS())), "LocGioi");
            KiemTra.Bang("SV03", b.Lay(b.Goi("TimDauTienDuoi5", DS()), "MSSV"), "TimDauTienDuoi5");
            KiemTra.Bang(false, b.Goi("CoAiDat10", DS()), "CoAiDat10");
        }

        [Fact(DisplayName = "B6 · SapXepTheoTen")]
        public void SapXep()
        {
            Array ds = DS();
            b.Goi("SapXepTheoTen", ds);
            KiemTra.Bang(new[] { "SV02", "SV03", "SV01", "SV04" }, SV.Ma(b, ds), "Thứ tự MSSV sau khi xếp theo họ tên");
        }
    }

    public class B7_MaTran
    {
        readonly BaiTap b = new("Buoi09_B7");

        [Fact(DisplayName = "B7 · Toán tử + và *")]
        public void ToanTu()
        {
            object x = b.Tao("MaTran", (object)new[,] { { 1, 2 }, { 3, 4 } }), y = b.Tao("MaTran", (object)new[,] { { 5, 6 }, { 7, 8 } });
            KiemTra.Bang(new[,] { { 6, 8 }, { 10, 12 } }, b.Lay(b.GoiToanTu("op_Addition", x, y), "DuLieu"), "a + b");
            KiemTra.Bang(new[,] { { 19, 22 }, { 43, 50 } }, b.Lay(b.GoiToanTu("op_Multiply", x, y), "DuLieu"), "a * b");
        }

        [Fact(DisplayName = "B7 · Sai kích thước ném ArgumentException")]
        public void SaiKichThuoc()
        {
            object x = b.Tao("MaTran", (object)new[,] { { 1, 2 }, { 3, 4 } }), z = b.Tao("MaTran", (object)new int[3, 3]);
            var m = x.GetType().GetMethod("op_Addition");
            KiemTra.Dung(m != null, "Thiếu toán tử +.");
            b.PhaiNem<ArgumentException>(() => m.Invoke(null, new[] { x, z }), "Cộng ma trận 2 × 2 với 3 × 3");
        }
    }

    public class B8_HangDoiVong
    {
        readonly BaiTap b = new("Buoi09_B8");

        [Fact(DisplayName = "B8 · Sức chứa 3: thêm, đầy, lấy, quay vòng")]
        public void KichBan()
        {
            Type t = b.Kieu("HangDoiVong").MakeGenericType(typeof(int));
            object q = b.TaoTuKieu(t, 3);
            foreach (int x in new[] { 1, 2, 3 }) KiemTra.Bang(true, b.GoiPhuongThucDoiTuong(q, "Them", x), $"Them({x})");
            KiemTra.Bang(false, b.GoiPhuongThucDoiTuong(q, "Them", 4), "Them(4) khi đầy");
            KiemTra.Bang(1, b.GoiPhuongThucDoiTuong(q, "Lay"), "Lay() lần 1");
            KiemTra.Bang(true, b.GoiPhuongThucDoiTuong(q, "Them", 4), "Them(4) sau khi lấy");
            KiemTra.Bang(3, b.Lay(q, "SoPhanTu"), "SoPhanTu");
            var con = new[] { b.GoiPhuongThucDoiTuong(q, "Lay"), b.GoiPhuongThucDoiTuong(q, "Lay"), b.GoiPhuongThucDoiTuong(q, "Lay") };
            KiemTra.Bang(new[] { 2, 3, 4 }, con.Select(Convert.ToInt32).ToArray(), "Lấy hết (FIFO)");
            KiemTra.Bang(0, b.Lay(q, "SoPhanTu"), "SoPhanTu khi rỗng");
        }
    }
}
