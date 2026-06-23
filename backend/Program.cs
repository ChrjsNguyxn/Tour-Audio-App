using System.Text;
using backend.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
// BẮT BUỘC: Authentication phải nằm trên Authorization
app.UseAuthentication(); // 1. Mày là ai? (Kiểm tra Token)
app.UseAuthorization();  // 2. Mày được phép làm gì? (Kiểm tra Quyền)

app.MapControllers();

app.Run();