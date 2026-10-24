using SLib23T1080033.BusinessLayer;
using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.DataLayer.SQLServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Đăng ký DbHelper
builder.Services.AddScoped<DbHelper>();

// Đăng ký AuthorRepository
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

// Đăng ký AuthorService
builder.Services.AddScoped<AuthorService>();

// Đăng ký PublisherRepository
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

// Đăng ký PublisherService
builder.Services.AddScoped<PublisherService>();

// Đăng ký LanguageRepository
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
// Đăng ký LanguageService
builder.Services.AddScoped<LanguageService>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
