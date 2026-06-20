using Microsoft.EntityFrameworkCore;
using Quan4.AdminApi.Models;

namespace Quan4.AdminApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
    }
}
