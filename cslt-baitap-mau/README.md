# Bài tập Cơ sở Lập trình (C#)

Repo này là nơi em làm và nộp toàn bộ bài thực hành của môn. Mỗi lần em mở **Pull Request** vào nhánh `main`, GitHub tự biên dịch và chạy bộ kiểm thử cho các buổi em vừa sửa, rồi ghi kết quả ngay trong Pull Request.

## Làm bài trong 6 bước

1. Tạo nhánh cho buổi: `git switch -c buoi-05`
2. Mở `BaiTapCSLT.sln`, viết code trong `Buoi05/Buoi05_A1/Program.cs` … (xóa dòng `throw new NotImplementedException();` của hàm em làm).
3. Chạy thử trên máy: `dotnet test KiemThu/Buoi05.KiemThu` (hoặc Test Explorer trong Visual Studio).
4. `git add .` → `git commit -m "Buổi 5: A1–A6"` → `git push -u origin buoi-05`
5. Trên GitHub: **Compare & pull request** → **Create pull request**.
6. Chờ dấu ✅ ở mục *Tổng hợp kết quả*, đọc bình luận kết quả, rồi **Merge pull request**.

## Quy định

- Giữ đúng tên project, tên hàm và chữ ký hàm có sẵn trong `Program.cs`.
- Hàm tính toán không đọc bàn phím; mọi project trong buổi phải biên dịch được.
- Không sửa `KiemThu/`, `.github/`, `Directory.Build.props`, `BaiTapCSLT.sln`.

Hướng dẫn đầy đủ (có hình): tài liệu *Hướng dẫn nộp bài có kiểm thử tự động (GitHub CI)* của giảng viên.

## Ký hiệu trong báo cáo

| Ký hiệu | Ý nghĩa |
| --- | --- |
| ✅ Đạt | Mọi test của bài đều đúng |
| 🟡 Làm dở | Một số hàm đã làm đúng, còn hàm chưa làm |
| ⬜ Chưa làm | Còn dòng `throw new NotImplementedException();` |
| ❌ Có lỗi | Có test sai kết quả, lỗi khi chạy, quá thời gian hoặc thiếu hàm |
