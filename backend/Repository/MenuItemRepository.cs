using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;

using Microsoft.Extensions.Configuration;
using backend.DTOs.MenuItemDTO;

namespace backend.Repository
{
    public class MenuItemRepository
    {
        private readonly string _connectionString;

        public MenuItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        // 1. Lấy danh sách món ăn theo ID Quán ăn
        public async Task<IEnumerable<MenuItemResponseDto>> GetMenuItemsByEateryIdAsync(int eateryId)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                SELECT 
                    id AS Id, eatery_id AS EateryId, name AS Name, 
                    description AS Description, price AS Price, 
                    image_path AS ImagePath, is_available AS IsAvailable, 
                    created_at AS CreatedAt
                FROM menu_items
                WHERE eatery_id = @EateryId
                ORDER BY id DESC";
            
            return await connection.QueryAsync<MenuItemResponseDto>(sql, new { EateryId = eateryId });
        }

        // 2. Thêm món ăn mới vào Menu
        public async Task<int> CreateMenuItemAsync(int eateryId, CreateMenuItemRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                INSERT INTO menu_items (eatery_id, name, description, price, image_path, is_available) 
                VALUES (@EateryId, @Name, @Description, @Price, @ImagePath, @IsAvailable);
                SELECT last_insert_rowid();";
            
            return await connection.ExecuteScalarAsync<int>(sql, new {
                EateryId = eateryId, request.Name, request.Description, 
                request.Price, request.ImagePath, request.IsAvailable
            });
        }

        // 3. Cập nhật thông tin món ăn
        public async Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                UPDATE menu_items 
                SET name = @Name, description = @Description, price = @Price, 
                    image_path = @ImagePath, is_available = @IsAvailable
                WHERE id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new { 
                Id = id, request.Name, request.Description, 
                request.Price, request.ImagePath, request.IsAvailable 
            });
            return affectedRows > 0;
        }

        // 4. Xóa món ăn
        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "DELETE FROM menu_items WHERE id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}