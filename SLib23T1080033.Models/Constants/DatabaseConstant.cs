namespace SLib23T1080033.Models.Constant;

/// <summary>
/// Tập hợp các hằng biểu diễn những giá trị cố định được quy định trong thiết kế cơ sở dữ liệu LibraryManagementDB.
/// Các giá trị này tương ứng với những cột có tập giá trị được giới hạn bằng ràng buộc CHECK hoặc được sử dụng làm giá trị mặc định.
/// Sử dụng các hằng này trong mã nguồn giúp tránh nhập sai chuỗi trạng thái và giữ cho giá trị gửi xuống SQL Server nhất quán.
/// </summary>
public static class DatabaseConstant
{
    #region BookCopies.CopyCondition

    /// <summary>
    /// Giá trị cho biết bản sao sách ở tình trạng mới; tương ứng với BookCopies.CopyCondition.
    /// </summary>
    public const string BOOKCOPIES_COPYCONDITION_NEW = "New";

    /// <summary>
    /// Giá trị cho biết bản sao sách ở tình trạng tốt; tương ứng với BookCopies.CopyCondition và là giá trị mặc định của cột này.
    /// </summary>
    public const string BOOKCOPIES_COPYCONDITION_GOOD = "Good";

    /// <summary>
    /// Giá trị cho biết bản sao sách ở tình trạng trung bình; tương ứng với BookCopies.CopyCondition.
    /// </summary>
    public const string BOOKCOPIES_COPYCONDITION_FAIR = "Fair";

    /// <summary>
    /// Giá trị cho biết bản sao sách ở tình trạng hư hỏng; tương ứng với BookCopies.CopyCondition.
    /// </summary>
    public const string BOOKCOPIES_COPYCONDITION_DAMAGED = "Damaged";

    #endregion

    #region BookCopies.CopyStatus

    /// <summary>
    /// Giá trị cho biết bản sao sách hiện có sẵn để được mượn; tương ứng với BookCopies.CopyStatus và là giá trị mặc định của cột này.
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_AVAILABLE = "Available";

    /// <summary>
    /// Giá trị cho biết bản sao sách đang được mượn; tương ứng với BookCopies.CopyStatus.
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_ONLOAN = "OnLoan";

    /// <summary>
    /// Giá trị cho biết bản sao sách đang được đặt trước; tương ứng với BookCopies.CopyStatus.
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_RESERVED = "Reserved";

    /// <summary>
    /// Giá trị cho biết bản sao sách bị mất; tương ứng với BookCopies.CopyStatus.
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_LOST = "Lost";

    /// <summary>
    /// Giá trị cho biết bản sao sách đã bị loại khỏi kho lưu thông; tương ứng với BookCopies.CopyStatus.
    /// </summary>
    public const string BOOKCOPIES_COPYSTATUS_WITHDRAWN = "Withdrawn";

    #endregion

    #region Members.Gender

    /// <summary>
    /// Giá trị giới tính Nam; tương ứng với Members.Gender.
    /// </summary>
    public const string MEMBERS_GENDER_MALE = "Male";

    /// <summary>
    /// Giá trị giới tính Nữ; tương ứng với Members.Gender.
    /// </summary>
    public const string MEMBERS_GENDER_FEMALE = "Female";

    /// <summary>
    /// Giá trị giới tính Khác; tương ứng với Members.Gender.
    /// </summary>
    public const string MEMBERS_GENDER_OTHER = "Other";

    #endregion

    #region Members.MemberStatus

    /// <summary>
    /// Giá trị cho biết thẻ độc giả đang hoạt động; tương ứng với Members.MemberStatus và là giá trị mặc định của cột này.
    /// </summary>
    public const string MEMBERS_MEMBERSTATUS_ACTIVE = "Active";

    /// <summary>
    /// Giá trị cho biết độc giả đang bị tạm ngưng quyền sử dụng; tương ứng với Members.MemberStatus.
    /// </summary>
    public const string MEMBERS_MEMBERSTATUS_SUSPENDED = "Suspended";

    /// <summary>
    /// Giá trị cho biết thẻ độc giả đã hết hạn; tương ứng với Members.MemberStatus.
    /// </summary>
    public const string MEMBERS_MEMBERSTATUS_EXPIRED = "Expired";

    /// <summary>
    /// Giá trị cho biết hồ sơ độc giả đã đóng; tương ứng với Members.MemberStatus.
    /// </summary>
    public const string MEMBERS_MEMBERSTATUS_CLOSED = "Closed";

    #endregion

    #region Loans.LoanStatus

    /// <summary>
    /// Giá trị cho biết phiếu mượn đang được xử lý; tương ứng với Loans.LoanStatus và là giá trị mặc định của cột này.
    /// </summary>
    public const string LOANS_LOANSTATUS_ACTIVE = "Active";

    /// <summary>
    /// Giá trị cho biết phiếu mượn đã hoàn tất; tương ứng với Loans.LoanStatus.
    /// </summary>
    public const string LOANS_LOANSTATUS_COMPLETED = "Completed";

    /// <summary>
    /// Giá trị cho biết phiếu mượn đã bị hủy; tương ứng với Loans.LoanStatus.
    /// </summary>
    public const string LOANS_LOANSTATUS_CANCELLED = "Cancelled";

    #endregion

    #region LoanItems.ItemStatus

    /// <summary>
    /// Giá trị cho biết đầu sách trong phiếu mượn đang được mượn và chưa được trả; tương ứng với LoanItems.ItemStatus và là giá trị mặc định của cột này.
    /// </summary>
    public const string LOANITEMS_ITEMSTATUS_BORROWED = "Borrowed";

    /// <summary>
    /// Giá trị cho biết đầu sách trong phiếu mượn đã được trả; tương ứng với LoanItems.ItemStatus.
    /// </summary>
    public const string LOANITEMS_ITEMSTATUS_RETURNED = "Returned";

    /// <summary>
    /// Giá trị cho biết đầu sách trong phiếu mượn được ghi nhận là đã mất; tương ứng với LoanItems.ItemStatus.
    /// </summary>
    public const string LOANITEMS_ITEMSTATUS_LOST = "Lost";

    #endregion

    #region LoanItems.ReturnCondition

    /// <summary>
    /// Giá trị cho biết tình trạng sách khi trả là mới; tương ứng với LoanItems.ReturnCondition.
    /// </summary>
    public const string LOANITEMS_RETURNCONDITION_NEW = "New";

    /// <summary>
    /// Giá trị cho biết tình trạng sách khi trả là tốt; tương ứng với LoanItems.ReturnCondition.
    /// </summary>
    public const string LOANITEMS_RETURNCONDITION_GOOD = "Good";

    /// <summary>
    /// Giá trị cho biết tình trạng sách khi trả ở mức trung bình; tương ứng với LoanItems.ReturnCondition.
    /// </summary>
    public const string LOANITEMS_RETURNCONDITION_FAIR = "Fair";

    /// <summary>
    /// Giá trị cho biết tình trạng sách khi trả là hư hỏng; tương ứng với LoanItems.ReturnCondition.
    /// </summary>
    public const string LOANITEMS_RETURNCONDITION_DAMAGED = "Damaged";

    #endregion
}



