using backend.Database;
using backend.Models;
using Dapper;

namespace backend.Repository;

public class CategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryAsync<Category>(
            "SELECT * FROM categories"
        );
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Category>(
            """
            SELECT *
            FROM categories
            WHERE id = @Id
            """,
            new { Id = id }
        );
    }

    public async Task<int> CreateAsync(Category category)
    {
        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            """
            INSERT INTO categories
            (
                name,
                description
            )
            VALUES
            (
                @Name,
                @Description
            );

            SELECT last_insert_rowid();
            """,
            category
        );
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        using var connection = _context.CreateConnection();

        var rows = await connection.ExecuteAsync(
            """
            UPDATE categories
            SET
                name = @Name,
                description = @Description
            WHERE id = @Id
            """,
            category
        );

        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        var rows = await connection.ExecuteAsync(
            """
            DELETE FROM categories
            WHERE id = @Id
            """,
            new { Id = id }
        );

        return rows > 0;
    }
}