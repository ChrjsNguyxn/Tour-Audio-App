using FoodMapAPI.Data;
using FoodMapAPI.Repository;
using FoodMapAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kết nối SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cho phép React gọi API (CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
var app = builder.Build();

// Seed dữ liệu mẫu
// Seed dữ liệu mẫu
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Owners.Any())
    {
        db.Owners.Add(new Owner
        {
            FullName = "Admin",
            Email = "admin@foodmap.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            PhoneNumber = "0909000000",
            CreatedAt = DateTime.Now
        });
        db.SaveChanges();
    }

    if (!db.Shops.Any())
    {
        var owner = db.Owners.First();
        db.Shops.AddRange(
            new Shop { Name = "Bún Num Bò Chóc chú Hai", Category = "nuoc", PriceRange = "30-40k", Description = "Món bún đặc trưng Khmer.", Latitude = 10.7716, Longitude = 106.6850, OwnerId = owner.Id },
            new Shop { Name = "Gỏi đu đủ cô Lan", Category = "anvat", PriceRange = "12-18k", Description = "Gỏi đu đủ chua cay truyền thống.", Latitude = 10.7720, Longitude = 106.6855, OwnerId = owner.Id },
            new Shop { Name = "Chè mâm Campuchia", Category = "trangmieng", PriceRange = "20-25k", Description = "Chè mâm đậu xanh sương sáo.", Latitude = 10.7712, Longitude = 106.6860, OwnerId = owner.Id },
            new Shop { Name = "Bánh canh cá lóc bà Tư", Category = "nuoc", PriceRange = "35-45k", Description = "Bánh canh cá lóc đồng tươi.", Latitude = 10.7708, Longitude = 106.6845, OwnerId = owner.Id },
            new Shop { Name = "Chè bưởi & bánh flan", Category = "trangmieng", PriceRange = "15-20k", Description = "Bánh flan gia truyền 3 đời.", Latitude = 10.7718, Longitude = 106.6858, OwnerId = owner.Id }
        );
        db.SaveChanges();
    }
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});
app.Run();