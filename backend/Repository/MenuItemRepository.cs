using backend.Database;
using backend.Models;
using Dapper;

namespace backend.Repository;

public class MenuItemRepository
{
    private readonly AppDbContext _context;

    public MenuItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuItem>>
        GetByEateryIdAsync(int eateryId)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryAsync<MenuItem>(
            """
            SELECT *
            FROM menu_items
            WHERE eatery_id = @EateryId
            """,
            new { EateryId = eateryId }
        );
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<MenuItem>(
            """
            SELECT *
            FROM menu_items
            WHERE id = @Id
            """,
            new { Id = id }
        );
    }

    public async Task<int> CreateAsync(MenuItem item)
    {
        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            """
            INSERT INTO menu_items
            (
                eatery_id,
                name,
                image_path,
                price,
                description
            )
            VALUES
            (
                @EateryId,
                @Name,
                @ImagePath,
                @Price,
                @Description
            );

            SELECT last_insert_rowid();
            """,
            item
        );
    }

    public async Task<bool> UpdateAsync(MenuItem item)
    {
        using var connection = _context.CreateConnection();

        var rows = await connection.ExecuteAsync(
            """
            UPDATE menu_items
            SET
                name = @Name,
                image_path = @ImagePath,
                price = @Price,
                description = @Description,
                is_available = @IsAvailable
            WHERE id = @Id
            """,
            item
        );

        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();

        var rows = await connection.ExecuteAsync(
            """
            DELETE FROM menu_items
            WHERE id = @Id
            """,
            new { Id = id }
        );

        return rows > 0;
    }
}