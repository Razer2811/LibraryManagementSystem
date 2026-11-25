using Microsoft.AspNetCore.Authentication.Cookies;
using SLib23T1080033.BusinessLayer;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.DataLayer.SQLServer;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký MVC
builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// Cấu hình xác thực bằng Cookie
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "SLib23T1080033.Auth";

        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// Đăng ký phân quyền
builder.Services.AddAuthorization();

// Đăng ký DbHelper
builder.Services.AddScoped<DbHelper>();

// Đăng ký AuthorRepository và AuthorService
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<AuthorService>();

// Đăng ký PublisherRepository và PublisherService
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<PublisherService>();

// Đăng ký LanguageRepository và LanguageService
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<LanguageService>();

// Đăng ký CategoryRepository và CategoryService
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();

// Đăng ký LocationRepository và LocationService
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<LocationService>();

// Đăng ký MemberTypeRepository và MemberTypeService
builder.Services.AddScoped<IMemberTypeRepository, MemberTypeRepository>();
builder.Services.AddScoped<MemberTypeService>();

// Đăng ký MemberRepository và MemberService
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<MemberService>();

// Đăng ký EmployeeRepository và EmployeeService
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<EmployeeService>();

var app = builder.Build();

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Xác thực người dùng trước khi kiểm tra quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();