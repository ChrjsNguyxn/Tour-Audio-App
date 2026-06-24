using System.Text;
using backend.Database;
using backend.Repository;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AppDBContext 
builder.Services.AddScoped<AppDbContext>();

// ==========================================================
// 1. ĐĂNG KÝ CÁC REPOSITORY (Dependency Injection)
// Đảm bảo Controller nào gọi Repo đó đều có hàng để dùng
// ==========================================================
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<EateryRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<MenuItemRepository>();
builder.Services.AddScoped<TouristRepository>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<MixedRepository>(); // repo cho POI

// Đăng ký Service
builder.Services.AddScoped<TouristService>(); // service cho tourist
// ==========================================================
// 2. CẤU HÌNH Ổ KHÓA BẢO MẬT (JWT TOKEN)
// ==========================================================
var key = Encoding.ASCII.GetBytes("KhoaBiMatCucKyDaiVaNguyHiemCuaDuAnFoodTourQuan4123456789");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5173",
                    "https://localhost:5173"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// ==========================================================
// 3. CẤU HÌNH PIPELINE (Thứ tự cực kỳ quan trọng)
// ==========================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Cho phép frontend gọi API
app.UseCors("AllowFrontend");

// BẮT BUỘC: Authentication phải nằm trên Authorization
app.UseAuthentication(); // 1. Mày là ai? (Kiểm tra Token)
app.UseAuthorization();  // 2. Mày được phép làm gì? (Kiểm tra Quyền)

app.MapControllers();

using (var connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=Database/foodtour.db"))
{
    connection.Open();
    var cmd = connection.CreateCommand();
    cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS reviews (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            eatery_id INTEGER NOT NULL,
            user_id INTEGER NOT NULL,
            rating INTEGER NOT NULL,
            comment TEXT,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP
        );";
    cmd.ExecuteNonQuery();
}

app.Run();