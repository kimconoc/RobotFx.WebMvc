using RobotFx.DoMain.FileLog;
using RobotFx.WebMvc.MemCached.Interface;
using RobotFx.WebMvc.MemCached;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RobotFx.Repositories.DBContext;
using RobotFx.Repositories.EntityFamework.Interface;
using RobotFx.Repositories.EntityFamework;
using RobotFx.DoMain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Xử dụng để lấy cooki
builder.Services.AddHttpContextAccessor();

// Xử dụng để lấy AddSession
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});

// Đăng ký DbContext với chuỗi kết nối
var connectionString = builder.Configuration.GetConnectionString("DbSqlRobotFx");
builder.Services.AddDbContext<CommonDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IMemCached, MemCached>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            // Chuyển hướng đến trang lỗi
            FileHelper.GeneratorFileByDay(ex.ToString(), MethodBase.GetCurrentMethod().Name);
            context.Response.Redirect("/Home/Error");
        }
    });
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Xử dụng để lấy AddSession
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();