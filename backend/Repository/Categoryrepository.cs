using Dapper;
using backend.Database;
using backend.Models;

namespace backend.Repository;

public class CategoryRepository
{
    private readonly AppDbContext _db;

    public CategoryRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// get all user
    /// </summary>
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Category>(
            "SELECT * FROM categories"
        );
    }

    /// <summary>
    /// get by id
    /// </summary>
    public async Task<Category?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Category>(
            "SELECT * FROM categories WHERE id = @Id",
            new { Id = id }
        );
    }

    /// <summary>
    /// get by category name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<Category?> GetByNameAsync(string name)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Category>(
            "SELECT * FROM categories WHERE name = @Name",
            new { Name = name }
        );
    }
}