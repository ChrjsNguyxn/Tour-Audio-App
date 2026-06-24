using System.Text;
<<<<<<< HEAD
using backend.Repository;
=======
using backend.Database;
using backend.Repository;
using backend.Services;
>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<< HEAD
=======
// Add AppDBContext 
builder.Services.AddScoped<AppDbContext>();

>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517
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
<<<<<<< HEAD
=======
builder.Services.AddScoped<MixedRepository>(); // repo cho POI

// Đăng ký Service
builder.Services.AddScoped<TouristService>(); // service cho tourist
>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517
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

<<<<<<< HEAD
=======
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

>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517
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
<<<<<<< HEAD
=======

// Cho phép frontend gọi API
app.UseCors("AllowFrontend");

>>>>>>> 9ac07cc6df21a2ba074a0e0cd1990a3cc86e9517
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