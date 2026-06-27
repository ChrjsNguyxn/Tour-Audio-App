// File này sẽ gồm các phưng thức cần tương tác giữa nhiều bảng với nhau
using Dapper;
using backend.DTOs;
using backend.Database;

namespace backend.Repository;

public class MixedRepository
{
    private readonly AppDbContext _connection;

    public MixedRepository(AppDbContext connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Lấy ra dữ liệu và xử lý thành POIResopnseDTO
    /// </summary>
    /// <returns></returns>
    public async Task<List<POIResponseDTO>> GetApprovedPOIsAsync()
    {
        using var connection = _connection.CreateConnection();

        // 1. Lấy Eatery và category name 
        var eaterySql = @"
            SELECT
                e.id,
                e.name,
                e.address,
                e.owner_id AS OwnerId,
                e.category_id AS CategoryId,
                c.name AS CategoryName,
                e.price_range AS PriceRange,
                e.description,
                e.rating,
                e.latitude,
                e.longitude,
                e.audio_file_path AS AudioFilePath,
                e.image_path AS ImagePath,
                e.open_time AS OpenTime,
                e.close_time AS CloseTime
            FROM eateries e
            JOIN categories c ON e.category_id = c.id
            WHERE e.is_approved = 1;
        ";

        var eateries = (await connection.QueryAsync<POIResponseDTO>(eaterySql))
            .ToList();

        if (!eateries.Any())
            return eateries;

        var eateryIds = eateries.Select(e => e.Id).ToList();

        // 2. Lấy menu item và map vào POI
        var menuSql = @"
            SELECT
                id,
                eatery_id AS EateryId,
                name,
                image_path AS ImagePath,
                price,
                description
            FROM menu_items
            WHERE is_available = 1
            AND eatery_id IN @Ids;
        ";

        var menuItems = (await connection.QueryAsync<POIMenuDTO>(
            menuSql,
            new { Ids = eateryIds }
        )).ToList();

        var menuLookup = menuItems
            .GroupBy(m => m.EateryId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var eatery in eateries)
        {
            eatery.MenuItems =
                menuLookup.TryGetValue(eatery.Id, out var menus)
                ? menus
                : new List<POIMenuDTO>();
        }

        return eateries;
    }

    // Lấy POI theo id 
    public async Task<POIResponseDTO?> GetApprovedPOIByIdAsync(int id)
    {
        using var connection = _connection.CreateConnection();

        var eaterySql = @"
            SELECT
                e.id,
                e.name,
                e.address,
                e.owner_id AS OwnerId,
                e.category_id AS CategoryId,
                c.name AS CategoryName,
                e.price_range AS PriceRange,
                e.description,
                e.rating,
                e.latitude,
                e.longitude,
                e.audio_file_path AS AudioFilePath,
                e.image_path AS ImagePath,
                e.open_time AS OpenTime,
                e.close_time AS CloseTime
            FROM eateries e
            JOIN categories c ON e.category_id = c.id
            WHERE e.id = @Id
            AND e.is_approved = 1;
        ";

        var poi = await connection.QueryFirstOrDefaultAsync<POIResponseDTO>(
            eaterySql,
            new { Id = id }
        );

        if (poi == null)
            return null;

        var menuSql = @"
            SELECT
                id,
                eatery_id AS EateryId,
                name,
                image_path AS ImagePath,
                price,
                description
            FROM menu_items
            WHERE eatery_id = @Id
            AND is_available = 1;
        ";

        var menuItems = await connection.QueryAsync<POIMenuDTO>(
            menuSql,
            new { Id = id }
        );

        poi.MenuItems = menuItems.ToList();

        return poi;
    }
}