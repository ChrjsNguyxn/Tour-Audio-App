
using Microsoft.EntityFrameworkCore;
using Quan4.AdminApi.Data;
using Scalar.AspNetCore; // Thêm thư viện Scalar
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Quan4.AdminApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Database SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Service vào hệ thống (thường đặt ngay dưới dòng AddDbContext)
builder.Services.AddScoped<VendorService>();

// 2. Thêm Controllers
builder.Services.AddControllers();

// MỞ CỬA CORS CHO FRONTEND REACT TRUY CẬP
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Cho phép React gọi sang
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerGen();

// Cấp "máy quét thẻ JWT" cho hệ thống để đối chiếu SecretKey
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("JwtSettings:SecretKey").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("=== LỖI GIẢI MÃ TOKEN ===");
            Console.WriteLine(context.Exception.Message);
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine("=== TOKEN BỊ TỪ CHỐI BỞI CHALLENGE ===");
            Console.WriteLine(context.Error + " - " + context.ErrorDescription);
            return Task.CompletedTask;
        }
    };
});

// 3. Khởi tạo OpenAPI chuẩn mới của .NET 10
builder.Services.AddOpenApi();

var app = builder.Build();

// 4. Cấu hình giao diện test API Scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Khởi chạy Scalar thay cho Swagger
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseCors("AllowReactApp"); // Kích hoạt CORS
app.UseAuthorization();

app.MapControllers();

app.Run();

