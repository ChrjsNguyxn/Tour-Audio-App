using backend.Database;
using backend.Repository; // Lưu ý: Namespace này phải khớp với chữ hoa/thường trong thư mục của bạn

using backend.Models;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Dapper (Tự động map snake_case trong DB sang PascalCase trong C#)
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// 2. Cấu hình CORS (Cho phép React từ cổng khác gọi API vào)
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. Đăng ký Database Context
builder.Services.AddSingleton<AppDbContext>();

// 4. Đăng ký các Repositories & Services
builder.Services.AddScoped<CategoryRepository>();

builder.Services.AddScoped<EateryRepository>();
builder.Services.AddScoped<MenuItemRepository>();
builder.Services.AddScoped<UserRepository>();

// 5. Đăng ký Controllers (Kích hoạt khả năng đọc API)
builder.Services.AddControllers();

var app = builder.Build();

// ==========================================
// KÍCH HOẠT MIDDLEWARE PIPELINE (Thứ tự rất quan trọng)
// ==========================================

// Bật CORS lên trước
app.UseCors("ReactPolicy"); 

// Ánh xạ các đường dẫn API tới các Controllers
app.MapControllers();

app.Run();