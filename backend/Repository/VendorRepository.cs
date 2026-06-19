using Dapper;
using backend.Database;
using backend.Models;

namespace backend.Repository;

public class VendorRepository
{
    private readonly AppDbContext _db;

    public VendorRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// get all vendor
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Vendor>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Vendor>(
            "SELECT * FROM vendors"
        );
    }

    /// <summary>
    /// get vendor by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Vendor?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Vendor>(
            "SELECT * FROM vendors WHERE id = @Id",
            new { Id = id }
        );
    }

    /// <summary>
    /// get vendor by owner id
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Vendor>> GetByOwnerIdAsync(int ownerId)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Vendor>(
            "SELECT * FROM vendors WHERE owner_id = @OwnerId",
            new { OwnerId = ownerId }
        );
    }

    /// <summary>
    /// get vendor by category id
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Vendor>> GetByCategoryIdAsync(int categoryId)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Vendor>(
            "SELECT * FROM vendors WHERE category_id = @CategoryId",
            new { CategoryId = categoryId }
        );
    }
}