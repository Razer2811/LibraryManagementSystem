using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SLib23T1080033.WebApp;

/// <summary>
/// Thông tin tài khoản người dùng được lưu trong phiên đăng nhập (cookie)
/// </summary>
public class WebUserData
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? IP { get; set; }
    public string? SessionId { get; set; }

    public List<string>? Roles { get; set; }

    /// <summary>
    /// Lấy danh sách các Claim chứa thông tin của user
    /// </summary>
    /// <returns></returns>
    private List<Claim> Claims
    {
        get
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(nameof(UserId), UserId ?? ""),
                new Claim(nameof(UserName), UserName ?? ""),
                new Claim(nameof(DisplayName), DisplayName ?? ""),
                new Claim(nameof(Email), Email ?? ""),
            };
            if (Roles != null)
                foreach (var role in Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));
            return claims;
        }
    }

    /// <summary>
    /// Tạo Principal dựa trên thông tin của người dùng
    /// </summary>
    /// <returns></returns>
    public ClaimsPrincipal CreatePrincipal()
    {
        var claimIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimPrincipal = new ClaimsPrincipal(claimIdentity);
        return claimPrincipal;
    }
}

/// <summary>
/// Định nghĩa tên của các role sử dụng trong phân quyền chức năng cho nhân viên
/// </summary>
public class WebUserRoles
{
    /// <summary>
    /// Quản trị
    /// </summary>
    public const string Administrator = "admin";
    /// <summary>
    /// Nhân viên
    /// </summary>
    public const string Employee = "employee";
}

/// <summary>
/// Extension các phương thức cho các đối tượng liên quan đến xác thực tài khoản người dùng
/// </summary>
public static class WebUserExtensions
{
    /// <summary>
    /// Đọc thông tin của user từ principal
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public static WebUserData? GetUserData(this ClaimsPrincipal principal)
    {
        try
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                return null;

            var userData = new WebUserData();

            userData.UserId = principal.FindFirstValue(nameof(userData.UserId));
            userData.UserName = principal.FindFirstValue(nameof(userData.UserName));
            userData.DisplayName = principal.FindFirstValue(nameof(userData.DisplayName));
            userData.Email = principal.FindFirstValue(nameof(userData.Email));
            userData.Roles = new List<string>();
            foreach (var claim in principal.FindAll(ClaimTypes.Role))
            {
                userData.Roles.Add(claim.Value);
            }

            return userData;
        }
        catch
        {
            return null;
        }

    }
}