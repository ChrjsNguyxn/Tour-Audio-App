using Dapper;
using backend.Database;
using backend.Models;

namespace backend.Repository;

public class MenuItemRepository
{
    private readonly AppDbContext _db;

    public MenuItemRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all menu items from database.
    /// </summary>
    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<MenuItem>(
            "SELECT * FROM menu_items"
        );
    }

    /// <summary>
    /// Get a menu item by its ID.
    /// Returns null if item does not exist.
    /// </summary>
    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<MenuItem>(
            "SELECT * FROM menu_items WHERE id = @Id",
            new { Id = id }
        );
    }

    /// <summary>
    /// Get all menu items belonging to a vendor.
    /// </summary>
    public async Task<IEnumerable<MenuItem>> GetByVendorIdAsync(int vendorId)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<MenuItem>(
            "SELECT * FROM menu_items WHERE vendor_id = @VendorId",
            new { VendorId = vendorId }
        );
    }

    /// <summary>
    /// Search menu items by name.
    /// Useful for search functionality.
    /// </summary>
    public async Task<IEnumerable<MenuItem>> SearchByNameAsync(string keyword)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<MenuItem>(
            "SELECT * FROM menu_items WHERE name LIKE @Keyword",
            new { Keyword = $"%{keyword}%" }
        );
    }

    /// <summary>
    /// Get menu items within a price range.
    /// </summary>
    public async Task<IEnumerable<MenuItem>> GetByPriceRangeAsync(
        int minPrice,
        int maxPrice)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<MenuItem>(
            @"SELECT * 
              FROM menu_items
              WHERE price BETWEEN @MinPrice AND @MaxPrice",
            new
            {
                MinPrice = minPrice,
                MaxPrice = maxPrice
            }
        );
    }
}