using System.Text;
using backend.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using backend.Database;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add AppDBContext 
builder.Services.AddScoped<AppDbContext>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<backend.Repository.AuthRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Cho phép mọi Frontend
              .AllowAnyMethod()  // Cho phép mọi lệnh GET, POST, PUT, DELETE
              .AllowAnyHeader(); // Cho phép mọi Header (bao gồm cả Token)
    });
});

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

builder.Services.AddScoped<OwnerStatsRepository>(); // repo mới cho Owner Dashboard (stats, update/delete quán)

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()   // Cho phép mọi tên miền/cổng
                   .AllowAnyMethod()   // Cho phép mọi lệnh GET, POST, PUT, DELETE, OPTIONS
                   .AllowAnyHeader();  // Cho phép mọi loại dữ liệu (JSON, Token...)
        });
});
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
builder.Services.AddTransient<backend.Database.AppDbContext>();
var app = builder.Build();
app.UseCors("AllowAll"); 
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<backend.Database.AppDbContext>();
    dbContext.SyncDatabaseSchema();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<backend.Database.AppDbContext>();
    dbContext.SyncDatabaseSchema();
}
// ==========================================================
// 3. CẤU HÌNH PIPELINE (Thứ tự cực kỳ quan trọng)
// ==========================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // cho phép truy cập file trong thư mục của backend

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


// ==========================================================
// MIGRATION CHO OWNER DASHBOARD (chỉ thêm bảng/cột mới,
// không xóa/sửa dữ liệu cũ, an toàn chạy lại nhiều lần)
// ==========================================================
using (var ownerMigrationConn = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=Database/foodtour.db"))
{
    ownerMigrationConn.Open();

    // 1) Bảng thống kê MỚI hoàn toàn — không đụng bảng cũ
    var createStatsTable = ownerMigrationConn.CreateCommand();
    createStatsTable.CommandText = @"
        CREATE TABLE IF NOT EXISTS eatery_stats (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            eatery_id INTEGER NOT NULL,
            event_type TEXT NOT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP
        );";
    createStatsTable.ExecuteNonQuery();

    // 2) Thêm 3 cột mới vào bảng eateries (chỉ nếu chưa có) — Dashboard.jsx cần
    var existingColumns = new System.Collections.Generic.HashSet<string>();
    var pragmaCmd = ownerMigrationConn.CreateCommand();
    pragmaCmd.CommandText = "PRAGMA table_info(eateries);";
    using (var reader = pragmaCmd.ExecuteReader())
    {
        while (reader.Read())
        {
            existingColumns.Add(reader.GetString(1)); // tên cột nằm ở index 1
        }
    }

    void AddColumnIfMissing(string columnName, string columnDef)
    {
        if (!existingColumns.Contains(columnName))
        {
            var alterCmd = ownerMigrationConn.CreateCommand();
            alterCmd.CommandText = $"ALTER TABLE eateries ADD COLUMN {columnName} {columnDef};";
            alterCmd.ExecuteNonQuery();
        }
    }

    AddColumnIfMissing("is_open_now", "INTEGER DEFAULT 1");
    AddColumnIfMissing("narration_text", "TEXT");
    AddColumnIfMissing("narration_language", "TEXT DEFAULT 'vi-VN'");
}

app.Run();