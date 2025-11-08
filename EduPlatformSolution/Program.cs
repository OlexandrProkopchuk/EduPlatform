// !!! ЗАМІНИ на свій namespace та класи:
using EduPlatform.Data;          // де лежить AppDbContext
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// якщо в тебе нема власного ApplicationUser — використовуй IdentityUser нижче

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext з рядком підключення "DefaultConnection"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Identity + ролі + зберігання в EF
builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>   // або ApplicationUser, якщо створював
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// 3) MVC і Razor Pages (Потрібно для Identity UI!)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ОБОВ’ЯЗКОВО: спочатку Authentication, потім Authorization
app.UseAuthentication();
app.UseAuthorization();

// Маршрути MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ОБОВ’ЯЗКОВО: підключити Razor Pages, інакше /Identity/... дає 404
app.MapRazorPages();

app.Run();
