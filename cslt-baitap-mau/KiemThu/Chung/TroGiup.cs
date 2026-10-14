// =====================================================================
//  TroGiup.cs – thư viện dùng chung cho mọi bộ kiểm thử CSLT.
//  KHÔNG SỬA TỆP NÀY. Sinh viên chỉ viết code trong thư mục BuoiNN/.
//
//  Cách hoạt động: bài làm của sinh viên được nạp bằng reflection theo
//  tên project (ví dụ "Buoi05_A3"); hàm được tìm theo tên và số tham số
//  nên hàm có để private (mặc định) vẫn gọi được. Mỗi lần gọi chạy trên
//  một luồng riêng, có giới hạn thời gian, bàn phím và màn hình giả lập.
// =====================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace KiemThu.Chung
{
    /// <summary>Một bài tập (một project BuoiNN_Xk) của sinh viên.</summary>
    public sealed class BaiTap
    {
        public const int ThoiGianToiDaMs = 5000;
        public string Ten { get; }
        public string ManHinh { get; private set; } = "";   // màn hình của lần chạy gần nhất
        private Assembly _asm;

        public BaiTap(string tenProject)
        {
            Ten = tenProject;
            try { _asm = Assembly.Load(new AssemblyName(tenProject)); }
            catch (Exception ex)
            {
                KiemTra.Loi($"[LỖI CẤU TRÚC] Không nạp được project {tenProject} ({ex.GetType().Name}). " +
                            "Kiểm tra em không đổi tên hay xóa project này.");
            }
        }

        // ------------------------------------------------------------ tìm kiểu, tìm hàm
        public Type Kieu(string ten)
        {
            Type t = _asm.GetTypes().FirstOrDefault(x => x.Name == ten || x.Name.StartsWith(ten + "`"));
            if (t == null)
                KiemTra.Loi($"[THIẾU KIỂU] Không tìm thấy kiểu (struct/class/enum/record) tên `{ten}` trong project {Ten}.");
            return t;
        }

        private IEnumerable<MethodInfo> TatCaHamStatic() =>
            _asm.GetTypes()
                .Where(t => !t.Name.StartsWith("<"))
                .OrderBy(t => t.Name == "Program" ? 0 : 1)
                .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                .Where(m => !m.Name.StartsWith("<"));

        internal static MethodInfo ChonHam(IEnumerable<MethodInfo> ungVien, string ten, object[] thamSo, string noiTim, Type[] kieuMongDoi = null)
        {
            var ds = ungVien.Where(m => m.Name == ten && m.GetParameters().Length == thamSo.Length).ToList();
            if (kieuMongDoi != null)
                ds = ds.Where(m => m.GetParameters().Select(p => p.ParameterType).SequenceEqual(kieuMongDoi)).ToList();
            MethodInfo tot = null; int diemTot = -1;
            foreach (var m in ds)
            {
                int d = DiemKhop(m.GetParameters(), thamSo);
                if (d > diemTot) { diemTot = d; tot = m; }
            }
            if (tot == null || diemTot < 0)
            {
                string chuKy = kieuMongDoi != null
                    ? $"{ten}({string.Join(", ", kieuMongDoi.Select(TenKieu))})"
                    : $"{ten}({string.Join(", ", thamSo.Select(a => a == null ? "?" : TenKieu(a.GetType())))})";
                var cungTen = ungVien.Where(m => m.Name == ten).Select(m => MoTaHam(m)).ToList();
                string goiY = cungTen.Count > 0
                    ? $" Em đang có: {string.Join("; ", cungTen)} – kiểm tra số lượng và kiểu tham số."
                    : " Kiểm tra tên hàm (phân biệt hoa thường) và từ khóa static.";
                KiemTra.Loi($"[THIẾU HÀM] Không tìm thấy hàm `{chuKy}` trong {noiTim}.{goiY}");
            }
            return tot;
        }

        // Điểm khớp: -1 = không gọi được; cao hơn = khớp chính xác hơn.
        private static int DiemKhop(ParameterInfo[] ps, object[] args)
        {
            int diem = 0;
            for (int i = 0; i < ps.Length; i++)
            {
                Type pt = ps[i].ParameterType.IsByRef ? ps[i].ParameterType.GetElementType() : ps[i].ParameterType;
                object a = args[i];
                if (a == null) { if (pt.IsValueType && !ps[i].IsOut && Nullable.GetUnderlyingType(pt) == null) return -1; diem += 1; continue; }
                if (a is Pointer) { if (pt.IsPointer) { diem += 3; continue; } return -1; }
                Type at = a.GetType();
                if (pt == at) diem += 3;
                else if (pt.IsAssignableFrom(at)) diem += 2;
                else if (LaKieuSo(pt) && LaKieuSo(at)) diem += 1;
                else return -1;
            }
            return diem;
        }

        internal static bool LaKieuSo(Type t) =>
            t == typeof(int) || t == typeof(long) || t == typeof(double) || t == typeof(float) ||
            t == typeof(decimal) || t == typeof(short) || t == typeof(byte) || t == typeof(uint) || t == typeof(ulong);

        internal static object[] ChuyenThamSo(MethodBase m, object[] args)
        {
            var ps = m.GetParameters();
            var kq = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                Type pt = ps[i].ParameterType.IsByRef ? ps[i].ParameterType.GetElementType() : ps[i].ParameterType;
                object a = args[i];
                if (a != null && !(a is Pointer) && pt != a.GetType() && LaKieuSo(pt) && LaKieuSo(a.GetType()))
                    a = Convert.ChangeType(a, pt, CultureInfo.InvariantCulture);
                kq[i] = a;
            }
            return kq;
        }

        internal static string TenKieu(Type t)
        {
            if (t == null) return "?";
            if (t.IsByRef) return "ref " + TenKieu(t.GetElementType());
            if (t.IsArray) return TenKieu(t.GetElementType()) + "[" + new string(',', t.GetArrayRank() - 1) + "]";
            if (t.IsPointer) return TenKieu(t.GetElementType()) + "*";
            if (t == typeof(int)) return "int"; if (t == typeof(long)) return "long"; if (t == typeof(double)) return "double";
            if (t == typeof(decimal)) return "decimal"; if (t == typeof(string)) return "string"; if (t == typeof(bool)) return "bool";
            if (t == typeof(char)) return "char"; if (t == typeof(void)) return "void"; if (t == typeof(float)) return "float";
            if (t.IsGenericType) return t.Name.Split('`')[0] + "<" + string.Join(", ", t.GetGenericArguments().Select(TenKieu)) + ">";
            return t.Name;
        }

        internal static string MoTaHam(MethodInfo m) =>
            $"{TenKieu(m.ReturnType)} {m.Name}({string.Join(", ", m.GetParameters().Select(p => (p.IsOut ? "out " + TenKieu(p.ParameterType.GetElementType()) : TenKieu(p.ParameterType))))})";

        // ------------------------------------------------------------ gọi hàm static
        /// <summary>Gọi hàm static. Mảng thamSo được cập nhật lại sau lời gọi (cho ref/out).</summary>
        public object Goi(string ten, params object[] thamSo) => GoiCoNhap(ten, null, thamSo);

        public object GoiCoNhap(string ten, string banPhim, params object[] thamSo)
        {
            MethodInfo m = ChonHam(TatCaHamStatic(), ten, thamSo, $"project {Ten}");
            return GoiPhuongThuc(m, null, thamSo, banPhim, MoTaLoiGoi(ten, thamSo));
        }

        public T Goi<T>(string ten, params object[] thamSo) => KiemTra.DoiKieu<T>(Goi(ten, thamSo), MoTaLoiGoi(ten, thamSo));

        /// <summary>Gọi hàm generic, ví dụ TimLonNhat&lt;int&gt;(…).</summary>
        public object GoiGeneric(string ten, Type[] kieuT, params object[] thamSo)
        {
            var ds = TatCaHamStatic().Where(m => m.Name == ten && m.IsGenericMethodDefinition &&
                                                  m.GetGenericArguments().Length == kieuT.Length &&
                                                  m.GetParameters().Length == thamSo.Length).ToList();
            if (ds.Count == 0)
                KiemTra.Loi($"[THIẾU HÀM] Không tìm thấy hàm generic `{ten}<{string.Join(", ", kieuT.Select(_ => "T"))}>` có {thamSo.Length} tham số trong project {Ten}.");
            MethodInfo mg;
            try { mg = ds[0].MakeGenericMethod(kieuT); }
            catch (ArgumentException) { KiemTra.Loi($"[SAI RÀNG BUỘC] Không dùng được {ten} với kiểu {string.Join(", ", kieuT.Select(TenKieu))}."); return null; }
            return GoiPhuongThuc(mg, null, thamSo, null, MoTaLoiGoi(ten, thamSo));
        }

        /// <summary>Chạy Main với dữ liệu bàn phím giả lập; trả về toàn bộ màn hình.</summary>
        public string ChayMain(string banPhim = "")
        {
            MethodInfo m = TatCaHamStatic().FirstOrDefault(x => x.Name == "Main");
            if (m == null) KiemTra.Loi($"[THIẾU HÀM] Project {Ten} không có hàm static void Main.");
            object[] ts = m.GetParameters().Length == 1 ? new object[] { new string[0] } : new object[0];
            GoiPhuongThuc(m, null, ts, banPhim ?? "", "Main");
            return ManHinh;
        }

        // ------------------------------------------------------------ đối tượng (buổi 9)
        public object Tao(string tenKieu, params object[] thamSo) => TaoTuKieu(Kieu(tenKieu), thamSo);

        public object TaoTuKieu(Type t, params object[] thamSo)
        {
            if (thamSo.Length == 0 && t.IsValueType) return Activator.CreateInstance(t);
            var ctor = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(c => c.GetParameters().Length == thamSo.Length)
                        .OrderByDescending(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(thamSo.Select(a => a?.GetType())) ? 1 : 0)
                        .FirstOrDefault();
            if (ctor == null)
                KiemTra.Loi($"[THIẾU HÀM TẠO] Kiểu {TenKieu(t)} không có hàm tạo nhận {thamSo.Length} tham số.");
            object[] ts = ChuyenThamSo(ctor, thamSo);
            return ChayCoGioiHan(() => ctor.Invoke(ts), null, $"new {TenKieu(t)}({string.Join(", ", thamSo.Select(BieuDien))})");
        }

        public object GoiPhuongThucDoiTuong(object doiTuong, string ten, params object[] thamSo)
        {
            if (doiTuong == null) KiemTra.Loi($"[LỖI KHI CHẠY] Gọi {ten} trên đối tượng null.");
            var ungVien = doiTuong.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo m = ChonHam(ungVien, ten, thamSo, $"kiểu {TenKieu(doiTuong.GetType())}");
            return GoiPhuongThuc(m, m.IsStatic ? null : doiTuong, thamSo, null, $"{TenKieu(doiTuong.GetType())}.{MoTaLoiGoi(ten, thamSo)}");
        }

        public object GoiToanTu(string tenToanTu, object x, object y)   // op_Addition, op_Multiply …
        {
            var ungVien = x.GetType().GetMethods(BindingFlags.Static | BindingFlags.Public);
            MethodInfo m = ChonHam(ungVien, tenToanTu, new[] { x, y }, $"kiểu {TenKieu(x.GetType())} (toán tử)");
            return GoiPhuongThuc(m, null, new[] { x, y }, null, $"toán tử {tenToanTu}");
        }

        /// <summary>Đọc trường hoặc thuộc tính theo tên.</summary>
        public object Lay(object doiTuong, string ten)
        {
            Type t = doiTuong.GetType();
            const BindingFlags F = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase;
            PropertyInfo p = t.GetProperty(ten, F);
            if (p != null && p.GetIndexParameters().Length == 0)
                return ChayCoGioiHan(() => p.GetValue(doiTuong), null, $"{TenKieu(t)}.{ten}");
            FieldInfo f = t.GetField(ten, F);
            if (f != null) return f.GetValue(doiTuong);
            KiemTra.Loi($"[THIẾU THÀNH PHẦN] Kiểu {TenKieu(t)} không có trường/thuộc tính `{ten}`.");
            return null;
        }

        /// <summary>Gán trường hoặc thuộc tính. Với struct, trả về bản sao đã gán.</summary>
        public object Gan(object doiTuong, string ten, object giaTri)
        {
            Type t = doiTuong.GetType();
            const BindingFlags F = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase;
            FieldInfo f = t.GetField(ten, F);
            if (f != null) { f.SetValue(doiTuong, ChuyenGiaTri(giaTri, f.FieldType)); return doiTuong; }
            PropertyInfo p = t.GetProperty(ten, F);
            if (p != null && p.CanWrite) { p.SetValue(doiTuong, ChuyenGiaTri(giaTri, p.PropertyType)); return doiTuong; }
            if (p != null)
            {   // thuộc tính chỉ đọc: thử trường sinh tự động
                FieldInfo bf = t.GetField($"<{p.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                if (bf != null) { bf.SetValue(doiTuong, ChuyenGiaTri(giaTri, bf.FieldType)); return doiTuong; }
            }
            KiemTra.Loi($"[THIẾU THÀNH PHẦN] Kiểu {TenKieu(t)} không có trường/thuộc tính `{ten}` gán được.");
            return null;
        }

        private static object ChuyenGiaTri(object v, Type t) =>
            v != null && LaKieuSo(t) && LaKieuSo(v.GetType()) ? Convert.ChangeType(v, t, CultureInfo.InvariantCulture) : v;

        /// <summary>Việc phải ném ngoại lệ kiểu TLoi (ví dụ ArgumentException khi mẫu số bằng 0).</summary>
        public void PhaiNem<TLoi>(Func<object> viec, string moTa) where TLoi : Exception
        {
            Exception loi = null;
            ChayCoGioiHan(() =>
            {
                try { viec(); }
                catch (TargetInvocationException e) { loi = e.InnerException; }
                catch (Exception e) when (!(e is QuaNhieuDauRaException || e is HetDuLieuNhapException)) { loi = e; }
                return null;
            }, null, moTa);
            if (loi is NotImplementedException)
                KiemTra.Loi($"[CHƯA LÀM] {moTa}: còn dòng `throw new NotImplementedException();`.");
            if (!(loi is TLoi))
                KiemTra.Loi($"[SAI KẾT QUẢ] {moTa} phải ném {typeof(TLoi).Name}, nhận được " +
                            (loi == null ? "không có lỗi nào." : $"{loi.GetType().Name}."));
        }

        // ------------------------------------------------------------ lõi chạy có giới hạn
        // Hàm đã quá giờ một lần thì các lần gọi sau báo lỗi ngay, không chờ thêm 5 giây.
        private static readonly HashSet<string> _daQuaGio = new();

        private object GoiPhuongThuc(MethodBase m, object doiTuong, object[] thamSo, string banPhim, string moTa)
        {
            string khoa = Ten + "." + m.DeclaringType?.Name + "." + m.Name;
            if (_daQuaGio.Contains(khoa))
                KiemTra.Loi($"[QUÁ THỜI GIAN] {moTa}: hàm {m.Name} đã chạy quá {ThoiGianToiDaMs / 1000} giây ở lần gọi trước " +
                            "(có thể bị vòng lặp vô hạn) nên không chạy lại.");
            object[] ts = ChuyenThamSo(m, thamSo);
            object kq;
            try { kq = ChayCoGioiHan(() => m.Invoke(doiTuong, ts), banPhim, moTa); }
            catch (Exception ex) when (ex.Message.StartsWith("[QUÁ THỜI GIAN]")) { _daQuaGio.Add(khoa); throw; }
            for (int i = 0; i < thamSo.Length; i++) thamSo[i] = ts[i];   // chép lại ref/out
            return kq;
        }

        public object ChayCoGioiHan(Func<object> viec, string banPhim, string moTa)
        {
            var phien = new PhienChay(banPhim ?? "");
            object kq = null; Exception loi = null;
            var luong = new Thread(() =>
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                PhienChay.DangKy(phien);
                try { kq = viec(); }
                catch (TargetInvocationException ex) { loi = ex.InnerException ?? ex; }
                catch (Exception ex) { loi = ex; }
                finally { PhienChay.HuyDangKy(); }
            }, 16 * 1024 * 1024) { IsBackground = true };

            ManHinhChung.CaiDat();
            luong.Start();
            bool xong = luong.Join(ThoiGianToiDaMs);
            ManHinh = phien.LayManHinh();
            if (!xong)
            {
                phien.HetHan = true;
                KiemTra.Loi($"[QUÁ THỜI GIAN] {moTa} chạy quá {ThoiGianToiDaMs / 1000} giây: có thể bị vòng lặp vô hạn " +
                            "hoặc chương trình đang chờ nhập thêm dữ liệu." + DoanCuoiManHinh());
            }
            if (loi != null) KiemTra.Loi(DichLoi(loi, moTa, phien) + DoanCuoiManHinh());
            return kq;
        }

        private string DoanCuoiManHinh()
        {
            if (string.IsNullOrEmpty(ManHinh)) return "";
            string s = ManHinh.Length > 300 ? "…" + ManHinh[^300..] : ManHinh;
            return "\nMàn hình cuối cùng:\n" + s;
        }

        private string DichLoi(Exception ex, string moTa, PhienChay phien)
        {
            if (ex is NotImplementedException)
                return $"[CHƯA LÀM] {moTa}: hàm vẫn còn dòng `throw new NotImplementedException();`. Xóa dòng đó và viết bài.";
            if (ex is QuaNhieuDauRaException)
                return $"[IN QUÁ NHIỀU] {moTa} in ra hơn {PhienChay.GioiHanKyTu:N0} ký tự: có thể bị vòng lặp vô hạn (ví dụ vòng nhập lại không dừng khi hết dữ liệu).";
            if (ex is HetDuLieuNhapException)
                return $"[THIẾU DỮ LIỆU NHẬP] {moTa} đọc bàn phím nhiều hơn số dòng của dữ liệu kiểm thử. Kiểm tra thứ tự và số lần nhập theo đúng đề.";
            if (ex is ArgumentNullException && phien.DaHetDuLieu)
                return $"[THIẾU DỮ LIỆU NHẬP] {moTa} đọc thêm dòng sau khi dữ liệu kiểm thử đã hết (Console.ReadLine() trả về null). Kiểm tra thứ tự nhập theo đề.";
            string viTri = ViTriTrongBaiLam(ex);
            string loai = ex switch
            {
                IndexOutOfRangeException => "truy cập ngoài phạm vi mảng",
                DivideByZeroException => "chia số nguyên cho 0",
                NullReferenceException => "dùng biến/đối tượng đang là null",
                FormatException => "chuyển chuỗi sang số thất bại – sai định dạng",
                OverflowException => "tràn số",
                FileNotFoundException => "không tìm thấy tệp",
                InvalidCastException => "ép kiểu không hợp lệ",
                StackOverflowException => "đệ quy không dừng",
                _ => ""
            };
            return $"[LỖI KHI CHẠY] {moTa} ném ra {ex.GetType().Name}" + (loai != "" ? $" ({loai})" : "") +
                   $": {ex.Message}" + (viTri != "" ? $"\nTại: {viTri}" : "");
        }

        private string ViTriTrongBaiLam(Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(ex, true);
            foreach (var fr in st.GetFrames() ?? Array.Empty<System.Diagnostics.StackFrame>())
            {
                var mb = fr.GetMethod();
                if (mb?.DeclaringType?.Assembly == _asm)
                {
                    string tep = fr.GetFileName();
                    int dong = fr.GetFileLineNumber();
                    return $"{mb.DeclaringType.Name}.{mb.Name}" + (tep != null ? $" ({Path.GetFileName(tep)}, dòng {dong})" : "");
                }
            }
            return "";
        }

        internal static string MoTaLoiGoi(string ten, object[] thamSo) =>
            $"{ten}({string.Join(", ", thamSo.Select(BieuDien))})";

        public static string BieuDien(object o)
        {
            switch (o)
            {
                case null: return "null";
                case string s: return "\"" + s + "\"";
                case char c: return "'" + c + "'";
                case double d: return d.ToString("R", CultureInfo.InvariantCulture);
                case bool b: return b ? "true" : "false";
                case Pointer: return "con trỏ";
                case int[,] m: return BieuDienMaTran(m);
                case double[,] m: return BieuDienMaTran(m);
                case Array a when a.Rank == 1:
                    return "{" + string.Join(", ", a.Cast<object>().Select(BieuDien)) + "}";
                case IFormattable f: return f.ToString(null, CultureInfo.InvariantCulture);
                default: return o.ToString();
            }
        }

        private static string BieuDienMaTran(Array m)
        {
            var dong = new List<string>();
            for (int i = 0; i < m.GetLength(0); i++)
            {
                var o = new List<string>();
                for (int j = 0; j < m.GetLength(1); j++) o.Add(BieuDien(m.GetValue(i, j)));
                dong.Add("{" + string.Join(", ", o) + "}");
            }
            return "{" + string.Join(", ", dong) + "}";
        }

        // ------------------------------------------------------------ thư mục tạm (buổi 8)
        /// <summary>Tạo thư mục tạm rỗng, đặt làm thư mục làm việc trong lúc chạy hành động.</summary>
        public static string ThuMucTam()
        {
            string d = Path.Combine(Path.GetTempPath(), "cslt-kt-" + Guid.NewGuid().ToString("N")[..8]);
            Directory.CreateDirectory(d);
            return d;
        }

        public string ChayMainTrongThuMuc(string thuMuc, string banPhim = "")
        {
            string cu = Environment.CurrentDirectory;
            try { Environment.CurrentDirectory = thuMuc; return ChayMain(banPhim); }
            finally { Environment.CurrentDirectory = cu; }
        }
    }

    // =====================================================================
    //  Bàn phím / màn hình giả lập, phân theo luồng.
    // =====================================================================
    internal sealed class PhienChay
    {
        public const int GioiHanKyTu = 200_000;
        private static readonly Dictionary<int, PhienChay> _theoLuong = new();
        private readonly StringBuilder _ra = new();
        private readonly StringReader _vao;
        private int _soLanDocNull;
        public volatile bool HetHan;
        public bool DaHetDuLieu => _soLanDocNull > 0;

        public PhienChay(string banPhim) { _vao = new StringReader(banPhim.Replace("\r\n", "\n")); }

        public static void DangKy(PhienChay p) { lock (_theoLuong) _theoLuong[Environment.CurrentManagedThreadId] = p; }
        public static void HuyDangKy() { lock (_theoLuong) _theoLuong.Remove(Environment.CurrentManagedThreadId); }
        /// <summary>Phiên của luồng hiện tại; null nếu luồng không thuộc lần chạy nào (ví dụ luồng của bộ chạy test).</summary>
        public static PhienChay HienTai
        {
            get
            {
                PhienChay p;
                lock (_theoLuong) if (!_theoLuong.TryGetValue(Environment.CurrentManagedThreadId, out p)) return null;
                if (p.HetHan) throw new PhienDaDungException();   // luồng quá giờ vẫn chạy: dừng nó lại
                return p;
            }
        }

        public void Ghi(string s)
        {
            lock (_ra)
            {
                if (_ra.Length + s.Length > GioiHanKyTu) throw new QuaNhieuDauRaException();
                _ra.Append(s);
            }
        }
        public string LayManHinh() { lock (_ra) return _ra.ToString().Replace("\r\n", "\n"); }

        public string DocDong()
        {
            string s = _vao.ReadLine();
            if (s == null && ++_soLanDocNull > 200) throw new HetDuLieuNhapException();
            if (s != null) Ghi(s + "\n");   // hiện lại phím đã gõ, giống màn hình thật
            return s;
        }
        public int DocKyTu() => _vao.Read();
        public int XemKyTu() => _vao.Peek();
    }

    public sealed class QuaNhieuDauRaException : Exception { }
    public sealed class HetDuLieuNhapException : Exception { }
    public sealed class PhienDaDungException : Exception { public PhienDaDungException() : base("Lần chạy đã bị dừng.") { } }

    internal static class ManHinhChung
    {
        private static RaDieuPhoi _ra;
        private static VaoDieuPhoi _vao;
        public static void CaiDat()
        {
            if (_ra == null) { _ra = new RaDieuPhoi(Console.Out); _vao = new VaoDieuPhoi(Console.In); }
            Console.SetOut(_ra);
            Console.SetIn(_vao);
        }
    }

    /// <summary>Màn hình chung: luồng bài làm ghi vào phiên của nó, luồng khác ghi ra màn hình gốc.</summary>
    internal sealed class RaDieuPhoi : TextWriter
    {
        private readonly TextWriter _goc;
        public RaDieuPhoi(TextWriter goc) { _goc = goc; }
        public override Encoding Encoding => Encoding.UTF8;
        private void Ghi(string s)
        {
            var p = PhienChay.HienTai;
            if (p == null) _goc.Write(s); else p.Ghi(s);
        }
        public override void Write(char value) => Ghi(value.ToString());
        public override void Write(string value) { if (value != null) Ghi(value); }
        public override void Write(char[] buffer, int index, int count) => Ghi(new string(buffer, index, count));
        public override void WriteLine() => Ghi("\n");
        public override void WriteLine(string value) => Ghi((value ?? "") + "\n");
        public override void Flush() { if (PhienChay.HienTai == null) _goc.Flush(); }
    }

    internal sealed class VaoDieuPhoi : TextReader
    {
        private readonly TextReader _goc;
        public VaoDieuPhoi(TextReader goc) { _goc = goc; }
        public override string ReadLine() { var p = PhienChay.HienTai; return p == null ? _goc.ReadLine() : p.DocDong(); }
        public override int Read() { var p = PhienChay.HienTai; return p == null ? _goc.Read() : p.DocKyTu(); }
        public override int Peek() { var p = PhienChay.HienTai; return p == null ? _goc.Peek() : p.XemKyTu(); }
    }

    // =====================================================================
    //  So sánh kết quả với thông báo tiếng Việt.
    // =====================================================================
    public static class KiemTra
    {
        public const double SaiSo = 1e-6;

        public static void Loi(string thongBao) => Assert.Fail(thongBao);

        public static T DoiKieu<T>(object v, string moTa)
        {
            if (v == null) return default;
            if (v is T t) return t;
            if (BaiTap.LaKieuSo(typeof(T)) && BaiTap.LaKieuSo(v.GetType()))
                return (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);
            if (typeof(T) == typeof(string)) return (T)(object)Convert.ToString(v, CultureInfo.InvariantCulture);
            Loi($"[SAI KIỂU TRẢ VỀ] {moTa} trả về kiểu {BaiTap.TenKieu(v.GetType())}, mong đợi {BaiTap.TenKieu(typeof(T))}.");
            return default;
        }

        public static void Bang(object mongDoi, object thucTe, string moTa)
        {
            if (!GiongNhau(mongDoi, thucTe))
                Loi($"[SAI KẾT QUẢ] {moTa}: mong đợi {BaiTap.BieuDien(mongDoi)}, nhận được {BaiTap.BieuDien(thucTe)}.");
        }

        public static void Gan(double mongDoi, double thucTe, double saiSo, string moTa)
        {
            if (double.IsNaN(thucTe) || Math.Abs(mongDoi - thucTe) > saiSo)
                Loi($"[SAI KẾT QUẢ] {moTa}: mong đợi {mongDoi.ToString("R", CultureInfo.InvariantCulture)} (sai số ≤ {saiSo:G3}), " +
                    $"nhận được {thucTe.ToString("R", CultureInfo.InvariantCulture)}.");
        }

        public static void Dung(bool dieuKien, string thongBao)
        {
            if (!dieuKien) Loi("[SAI KẾT QUẢ] " + thongBao);
        }

        /// <summary>Chuỗi bằng nhau, bỏ qua hoa thường và khoảng trắng thừa.</summary>
        public static void ChuoiBang(string mongDoi, string thucTe, string moTa)
        {
            if (ChuanHoa(mongDoi) != ChuanHoa(thucTe))
                Loi($"[SAI KẾT QUẢ] {moTa}: mong đợi \"{mongDoi}\", nhận được \"{thucTe}\".");
        }

        public static string ChuanHoa(string s) =>
            Regex.Replace((s ?? "").Trim().ToLowerInvariant(), @"\s+", " ").Normalize(NormalizationForm.FormC);

        /// <summary>Màn hình phải chứa đoạn văn bản (so khớp chính xác từng ký tự).</summary>
        public static void Chua(string manHinh, string doan, string moTa)
        {
            if (manHinh == null || !manHinh.Normalize(NormalizationForm.FormC).Contains(doan.Normalize(NormalizationForm.FormC)))
                Loi($"[SAI KẾT QUẢ] {moTa}: màn hình không có \"{doan}\".\nMàn hình của em:\n{CatNgan(manHinh)}");
        }

        /// <summary>Màn hình chứa đoạn văn bản, không phân biệt hoa thường/khoảng trắng.</summary>
        public static void ChuaGanDung(string manHinh, string doan, string moTa)
        {
            if (!ChuanHoa(manHinh).Contains(ChuanHoa(doan)))
                Loi($"[SAI KẾT QUẢ] {moTa}: màn hình không có \"{doan}\".\nMàn hình của em:\n{CatNgan(manHinh)}");
        }

        public static void KhongChua(string manHinh, string doan, string moTa)
        {
            if (manHinh != null && manHinh.Contains(doan))
                Loi($"[SAI KẾT QUẢ] {moTa}: màn hình không được có \"{doan}\".\nMàn hình của em:\n{CatNgan(manHinh)}");
        }

        /// <summary>Các số xuất hiện trên màn hình (bỏ dấu phân cách hàng nghìn).</summary>
        public static List<double> CacSo(string manHinh)
        {
            var ds = new List<double>();
            foreach (Match m in Regex.Matches(manHinh ?? "", @"-?\d{1,3}(?:,\d{3})+(?:\.\d+)?|-?\d+(?:\.\d+)?"))
                ds.Add(double.Parse(m.Value.Replace(",", ""), CultureInfo.InvariantCulture));
            return ds;
        }

        /// <summary>Số cuối cùng trên màn hình phải bằng giá trị mong đợi.</summary>
        public static void SoCuoi(string manHinh, double mongDoi, string moTa)
        {
            var so = CacSo(manHinh);
            if (so.Count == 0 || Math.Abs(so[^1] - mongDoi) > SaiSo)
                Loi($"[SAI KẾT QUẢ] {moTa}: số cuối cùng in ra phải là {mongDoi.ToString(CultureInfo.InvariantCulture)}" +
                    (so.Count == 0 ? ", nhưng màn hình không có số nào." : $", nhận được {so[^1].ToString(CultureInfo.InvariantCulture)}.") +
                    $"\nMàn hình của em:\n{CatNgan(manHinh)}");
        }

        /// <summary>So từng dòng, bỏ khoảng trắng cuối dòng và dòng trống ở cuối.</summary>
        public static void HinhBang(string mongDoi, string manHinh, string moTa)
        {
            string a = BoCuoiDong(mongDoi), b = BoCuoiDong(manHinh);
            if (a != b)
                Loi($"[SAI KẾT QUẢ] {moTa}: hình in ra chưa đúng.\nMong đợi:\n{a}\nEm in ra:\n{CatNgan(b)}");
        }

        public static string BoCuoiDong(string s) =>
            string.Join("\n", (s ?? "").Replace("\r\n", "\n").Split('\n').Select(x => x.TrimEnd())).TrimEnd('\n');

        private static string CatNgan(string s)
        {
            s ??= "";
            return s.Length > 600 ? s[..600] + "\n…(còn nữa)" : s;
        }

        public static bool GiongNhau(object a, object b)
        {
            if (a == null || b == null) return a == null && b == null;
            if (BaiTap.LaKieuSo(a.GetType()) && BaiTap.LaKieuSo(b.GetType()))
            {
                double x = Convert.ToDouble(a, CultureInfo.InvariantCulture), y = Convert.ToDouble(b, CultureInfo.InvariantCulture);
                return Math.Abs(x - y) <= SaiSo * Math.Max(1, Math.Abs(x));
            }
            if (a is string sa && b is string sb) return sa == sb;
            if (a is Array aa && b is Array ab)
            {
                if (aa.Rank != ab.Rank) return false;
                for (int r = 0; r < aa.Rank; r++) if (aa.GetLength(r) != ab.GetLength(r)) return false;
                var ea = aa.Cast<object>().ToList(); var eb = ab.Cast<object>().ToList();
                for (int i = 0; i < ea.Count; i++) if (!GiongNhau(ea[i], eb[i])) return false;
                return true;
            }
            if (a is Enum || b is Enum) return a.ToString() == b.ToString();
            return a.Equals(b);
        }
    }
}
