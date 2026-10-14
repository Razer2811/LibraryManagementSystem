# LibraryManagementSystem
Library Management System built with ASP.NET Core MVC, Clean Architecture, and SQL Server for the Web Application Programming course at HUSC.
# 📚 Hệ Thống Quản Lý Thư Viện (Library Management System)

> **Dự án cuối kỳ học phần Lập trình ứng dụng Web**  
> **Trường Đại học Khoa học - Đại học Huế | Khoa Công nghệ Thông tin**

---

## 📌 1. Thông tin chung về Học phần

* **Tên học phần:** Lập trình ứng dụng Web (Web Application Programming)
* **Mã học phần:** TIN4613
* **Số tín chỉ:** 3 tín chỉ (45 tiết)
* **Đơn vị phụ trách:** Bộ môn Công nghệ phần mềm - Khoa Công nghệ thông tin
* **Phương pháp học tập:** Học theo dự án (Project-Based Learning - PBL)
* **Giảng viên hướng dẫn:**
  * ThS. Trần Nguyên Phong (Email: tnphong@husc.edu.vn)
  * TS. Nguyễn Hoàng Hà (Email: nhha@husc.edu.vn)

---

## 🎯 2. Tổng quan về Dự án (Project Overview)

Dự án tập trung xây dựng một **Ứng dụng Web Quản lý Thư viện hoàn chỉnh** (bao gồm Frontend, Backend và Cơ sở dữ liệu), phục vụ cho các hoạt động quản lý danh mục sách, theo dõi kho sách vật lý, quản lý thẻ bạn đọc và thực hiện các giao dịch mượn - trả sách.

### 🏛️ Kiến trúc Hệ thống (Architecture)
Hệ thống được thiết kế và triển khai theo mô hình **Clean Architecture** (hoặc **3-Layer Architecture**) kết hợp với mô hình **MVC (Model-View-Controller)** trên nền tảng **[đã xoá đường liên kết đáng ngờ] Core**:

* **Domain Layer:** Chứa các thực thể cốt lõi (Entities) và logic nghiệp vụ thuần túy.
* **Application Layer:** Định nghĩa các Interface Repository, DTOs (Data Transfer Objects) và Application Services / Use Cases.
* **Infrastructure Layer:** Xử lý kết nối và truy xuất cơ sở dữ liệu SQL Server (sử dụng [đã xoá đường liên kết đáng ngờ] / Entity Framework Core) triển khai các Interface từ Application Layer.
* **Presentation Layer:** Giao diện người dùng Web ([đã xoá đường liên kết đáng ngờ] Core MVC Controller & Razor Views).

---

## 🗄️ 3. Mô hình Cơ sở dữ liệu (`LibraryManagementDB`)

Cơ sở dữ liệu được thiết kế gồm **14 bảng** chuẩn hóa trên **Microsoft SQL Server**, chia làm 5 nhóm nghiệp vụ chính:

1. **Nhóm Danh mục (Catalogue Metadata):** `Publishers`, `Authors`, `Categories` (cấu trúc cây phân cấp), `Languages`.
2. **Nhóm Tài liệu & Biên mục (Book Cataloging):** `Books`, `BookAuthors` (quan hệ N-N), `BookCategories` (quan hệ N-N).
3. **Nhóm Kho lưu trữ (Inventory):** `Locations` (vị trí Tầng/Phòng/Kệ), `BookCopies` (quản lý từng cuốn sách vật lý theo mã vạch `Barcode` riêng).
4. **Nhóm Bạn đọc & Thẻ (Members):** `MemberTypes` (quy định chính sách mượn/số ngày mượn tối đa), `Members` (thông tin thẻ và trạng thái thẻ).
5. **Nhóm Quản lý Lưu thông (Circulation):** `Loans` (phiếu mượn header), `LoanItems` (chi tiết mượn/trả/báo mất từng cuốn sách).
6. **Nhóm Quản trị Nhân sự:** `Employees` (quản lý thông tin thủ thư/nhân viên lập phiếu).

---

## 🚀 4. Các Chức năng chính (Key Features)

- [x] **Quản lý Danh mục & Đầu sách:** Thêm, sửa, xóa, tìm kiếm nhà xuất bản, tác giả, thể loại và thông tin biên mục đầu sách.
- [x] **Quản lý Bản sao & Kho sách:** Gán mã vạch (`Barcode`) riêng cho từng cuốn sách vật lý, theo dõi vị trí lưu trữ và trạng thái sách (`Available`, `OnLoan`, `Lost`,...).
- [x] **Quản lý Bạn đọc:** Quản lý thông tin bạn đọc, phân loại thẻ (Sinh viên/Giảng viên), theo dõi ngày hết hạn và trạng thái thẻ.
- [x] **Xử lý Mượn - Trả sách (Circulation Workflow):**
  - Tạo phiếu mượn mới (ràng buộc không mượn trùng cuốn sách đang active).
  - Xử lý trả sách, cập nhật tình trạng sách khi trả (`New`, `Good`, `Fair`, `Damaged`).
  - Ghi nhận báo mất sách và tự động chuyển trạng thái phiếu mượn sang `Completed`.
- [x] **Quản trị Nhân viên:** Đăng nhập, phân quyền thao tác cho thủ thư.

---

## 🛠️ 5. Công nghệ sử dụng (Tech Stack)

* **Backend:** C# / .NET / [đã xoá đường liên kết đáng ngờ] Core MVC
* **Architecture / Patterns:** Clean Architecture / 3-Layer, Repository Pattern, Dependency Injection (DI)
* **Database:** Microsoft SQL Server
* **Data Access:** [đã xoá đường liên kết đáng ngờ] / Entity Framework Core
* **Frontend:** HTML5, CSS3, JavaScript, Bootstrap / Razor Views
* **Version Control:** Git & GitHub

---

## 📊 6. Cơ cấu Đánh giá & Chấm điểm Học phần

| Thành phần đánh giá | Hình thức / Tiêu chí | Trọng số |
| :--- | :--- | :---: |
| **Đánh giá quá trình (40%)** | • Chuyên cần & Thái độ học tập<br>• Tiến độ thực hành & Báo cáo sơ bộ giữa kỳ (Buổi 10) | **10%**<br>**30%** |
| **Đánh giá cuối kỳ (60%)** | • Mức độ hoàn thành chức năng dự án (>85%)<br>• Chất lượng kỹ thuật & Mã nguồn chuẩn<br>• Giao diện & Trải nghiệm người dùng (UI/UX)<br>• Báo cáo & Trình bày sản phẩm trực tiếp (Buổi 15) | **60%** |

---


