using Dapper;
using backend.Database;
using backend.Models;

namespace backend.Repository;

public class EateryRepository
{
    private readonly AppDbContext _db;

    public EateryRepository(AppDbContext db)
    {
        _db = db;
    }

    //======
    // GET
    //======

    /// <summary>
    /// get all eatery
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Eatery>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Eatery>(
            "SELECT * FROM eateries"
        );
    }

    /// <summary>
    /// get Eatery by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Eatery?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Eatery>(
            "SELECT * FROM eateries WHERE id = @Id",
            new { Id = id }
        );
    }

    /// <summary>
    /// get Eatery by owner id
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Eatery>> GetByOwnerIdAsync(int ownerId)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Eatery>(
            "SELECT * FROM eateries WHERE owner_id = @OwnerId",
            new { OwnerId = ownerId }
        );
    }

    /// <summary>
    /// get Eatery by category id
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Eatery>> GetByCategoryIdAsync(int categoryId)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<Eatery>(
            "SELECT * FROM eateries WHERE category_id = @CategoryId",
            new { CategoryId = categoryId }
        );
    }

    /// <summary>
    /// get approved Eatery 
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Eatery>> GetApprovedAsync()
    {
        using var connection = _db.CreateConnection();

        string sql = """
            SELECT *
            FROM eateries
            WHERE is_approved = 1
            """;

        return await connection.QueryAsync<Eatery>(sql);
    }

    //========
    // CREATE
    //========

    /// <summary>
    /// create eatery
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<int> CreateAsync(Eatery eatery)
    {
        using var connection = _db.CreateConnection();

        string sql = """
            INSERT INTO eateries
            (
                name,
                address,
                owner_id,
                category_id,
                price_range,
                description,
                latitude,
                longitude,
                audio_file_path,
                image_path,
                open_time,
                close_time
            )
            VALUES
            (
                @Name,
                @Address,
                @OwnerId,
                @CategoryId,
                @PriceRange,
                @Description,
                @Latitude,
                @Longitude,
                @AudioFilePath,
                @ImagePath,
                @OpenTime,
                @CloseTime
            );

            SELECT last_insert_rowid();
            """;

        return await connection.ExecuteScalarAsync<int>(
            sql,
            eatery
        );
    }

    //==========
    // UPDATE
    //==========

    /// <summary>
    /// update an eatery
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(Eatery eatery)
    {
        using var connection = _db.CreateConnection();

        string sql = """
            UPDATE eateries
            SET
                name = @Name,
                address = @Address,
                category_id = @CategoryId,
                price_range = @PriceRange,
                description = @Description,
                latitude = @Latitude,
                longitude = @Longitude,
                audio_file_path = @AudioFilePath,
                image_path = @ImagePath,
                open_time = @OpenTime,
                close_time = @CloseTime
            WHERE id = @Id
            """;

        int rows = await connection.ExecuteAsync(sql, eatery);

        return rows > 0;
    }

    //==========
    // APPROVE
    //==========

    /// <summary>
    /// admin use this to approve an eatery
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<bool> ApproveAsync(int id)
    {
        using var connection = _db.CreateConnection();

        string sql = """
            UPDATE eateries
            SET is_approved = 1
            WHERE id = @Id
            """;

        int rows = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return rows > 0;
    }

    //=========
    // DELETE
    //=========

    /// <summary>
    /// delete an eatery from the db
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.CreateConnection();

        string sql = """
            DELETE FROM eateries
            WHERE id = @Id
            """;

        int rows = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return rows > 0;
    }
}