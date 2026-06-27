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

        // [MỚI THÊM] - Lấy TẤT CẢ món ăn cho trang Admin
        public async Task<IEnumerable<MenuItemResponseDto>> GetAllMenuItemsAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                SELECT 
                    id AS Id, eatery_id AS EateryId, name AS Name, 
                    description AS Description, price AS Price, 
                    image_path AS ImagePath, audio_path AS AudioPath, is_available AS IsAvailable, 
                    created_at AS CreatedAt
                FROM menu_items
                ORDER BY id DESC";
            return await connection.QueryAsync<MenuItemResponseDto>(sql);
        }

        // 1. Lấy danh sách món ăn theo ID Quán ăn (Giữ nguyên cho App)
        public async Task<IEnumerable<MenuItemResponseDto>> GetMenuItemsByEateryIdAsync(int eateryId)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                SELECT 
                    id AS Id, eatery_id AS EateryId, name AS Name, 
                    description AS Description, price AS Price, 
                    image_path AS ImagePath, audio_path AS AudioPath, is_available AS IsAvailable, 
                    created_at AS CreatedAt
                FROM menu_items
                WHERE eatery_id = @EateryId
                ORDER BY id DESC";
            return await connection.QueryAsync<MenuItemResponseDto>(sql, new { EateryId = eateryId });
        }

        // 2. Thêm món ăn mới vào Menu
        public async Task<int> CreateMenuItemAsync(CreateMenuItemRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                INSERT INTO menu_items (eatery_id, name, description, price, image_path, audio_path, is_available) 
                VALUES (@EateryId, @Name, @Description, @Price, @ImagePath, @AudioPath, @IsAvailable);
                SELECT last_insert_rowid();";
            
            return await connection.ExecuteScalarAsync<int>(sql, request);
        }

        // 3. Cập nhật thông tin món ăn
        public async Task<bool> UpdateMenuItemAsync(int id, UpdateMenuItemRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                UPDATE menu_items 
                SET eatery_id = @EateryId, name = @Name, description = @Description, price = @Price, 
                    image_path = @ImagePath, audio_path = @AudioPath, is_available = @IsAvailable
                WHERE id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new { 
                Id = id, request.EateryId, request.Name, request.Description, 
                request.Price, request.ImagePath, request.AudioPath, request.IsAvailable 
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

        // 5. Lấy danh sách món ăn đang được phục vụ(is_available = 1)
        public async Task<IEnumerable<MenuItemResponseDto>> 
        GetAvailableMenuItemsByEateryIdAsync(int eateryId)
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
                AND is_available = 1
                ORDER BY id DESC";

            return await connection.QueryAsync<MenuItemResponseDto>(sql, new { EateryId = eateryId });
        }
    }
}