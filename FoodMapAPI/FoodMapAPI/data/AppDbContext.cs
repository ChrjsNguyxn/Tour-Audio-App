using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Models;

namespace FoodMapAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<ViewLog> ViewLogs { get; set; }
    }
}