using backend.Database;
using backend.Repository;
using backend.Services;
using backend.Models;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// các này để các lớp trong Repository/ tự chuyển đổi tên các trường sang cho lớp trong Models/ 
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// cái này để cho phép frontend fetch dữ liệu từ server
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//
builder.Services.AddSingleton<AppDbContext>();

builder.Services.AddSingleton<AppDbContext>();

// EATERY
builder.Services.AddScoped<EateryRepository>();// Repository/
builder.Services.AddScoped<EateryService>();// Service/

// CATEGORY
builder.Services.AddScoped<CategoryRepository>();

// MENUITEM
builder.Services.AddScoped<MenuItemRepository>();

// USER
builder.Services.AddScoped<UserRepository>();

//
builder.Services.AddControllers();

//
var app = builder.Build();

app.UseCors("ReactPolicy"); // cho phép frontend fetch dữ liệu

app.MapControllers();


app.Run();