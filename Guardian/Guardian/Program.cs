using Microsoft.EntityFrameworkCore;
using Guardian.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add this for Web API
builder.Services.AddControllersWithViews();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDbContext<GuardianContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Authentication/Login";
        options.AccessDeniedPath = "/Authentication/Login";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Login");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll"); // Add CORS middleware

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Add endpoints mapping
app.MapControllers(); // This is crucial for Web API to work
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();