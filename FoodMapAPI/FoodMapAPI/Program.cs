using FoodMapAPI.Data;
using FoodMapAPI.Models;
using FoodMapAPI.Repository;
using FoodMapAPI.Service;
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

// Repository
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IEateryRepository, EateryRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

// Service
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IEateryService, EateryService>();

var app = builder.Build();

// Seed dữ liệu mẫu
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    Owner owner;
    if (!db.Owners.Any())
    {
        owner = new Owner
        {
            FullName = "Admin",
            Email = "admin@foodmap.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            PhoneNumber = "0909000000",
            Status = "Approved",
            CreatedAt = DateTime.Now
        };
        db.Owners.Add(owner);
        db.SaveChanges();
    }
    else
    {
        owner = db.Owners.First();
    }

    // Thêm category nào còn thiếu (không xóa cái đã có, không bị lỗi biến chưa gán)
    var wantedCategories = new[]
    {
        new Category { Name = "Món nước", Description = "Bún, phở, hủ tiếu..." },
        new Category { Name = "Ăn vặt", Description = "Món ăn vặt đường phố" },
        new Category { Name = "Tráng miệng", Description = "Chè, bánh, đồ ngọt" }
    };

    var existingNames = db.Categories.Select(c => c.Name).ToHashSet();
    foreach (var c in wantedCategories)
    {
        if (!existingNames.Contains(c.Name))
            db.Categories.Add(c);
    }
    db.SaveChanges();

    var category = db.Categories.First();

    if (!db.Eateries.Any())
    {
        db.Eateries.AddRange(
            new Eatery { Name = "Bún Num Bò Chóc chú Hai", CategoryId = category.Id, PriceRange = "30-40k", Description = "Món bún đặc trưng Khmer.", Latitude = 10.7716, Longitude = 106.6850, OwnerId = owner.Id },
            new Eatery { Name = "Gỏi đu đủ cô Lan", CategoryId = category.Id, PriceRange = "12-18k", Description = "Gỏi đu đủ chua cay truyền thống.", Latitude = 10.7720, Longitude = 106.6855, OwnerId = owner.Id },
            new Eatery { Name = "Chè mâm Campuchia", CategoryId = category.Id, PriceRange = "20-25k", Description = "Chè mâm đậu xanh sương sáo.", Latitude = 10.7712, Longitude = 106.6860, OwnerId = owner.Id },
            new Eatery { Name = "Bánh canh cá lóc bà Tư", CategoryId = category.Id, PriceRange = "35-45k", Description = "Bánh canh cá lóc đồng tươi.", Latitude = 10.7708, Longitude = 106.6845, OwnerId = owner.Id },
            new Eatery { Name = "Chè bưởi & bánh flan", CategoryId = category.Id, PriceRange = "15-20k", Description = "Bánh flan gia truyền 3 đời.", Latitude = 10.7718, Longitude = 106.6858, OwnerId = owner.Id }
        );
        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});

app.MapControllers();
app.Run();