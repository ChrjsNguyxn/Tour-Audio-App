using backend.Database;
using backend.Repository;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddSingleton<AppDbContext>();

builder.Services.AddSingleton<AppDbContext>();

builder.Services.AddScoped<VendorRepository>();

//
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

//
using (var scope = app.Services.CreateScope())
{
    var vendorRepo =
        scope.ServiceProvider.GetRequiredService<VendorRepository>();

    var vendors = await vendorRepo.GetAllAsync();

    foreach (var vendor in vendors)
    {
        Console.WriteLine(
            $"[{vendor.Id}] [{vendor.Name}]"
        );
    }
}
app.Run();