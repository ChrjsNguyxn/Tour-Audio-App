using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using backend.DTOs.EateryDTO;
using Microsoft.Extensions.Configuration;

namespace backend.Repository
{
    public class EateryRepository
    {
        private readonly string _connectionString;

        public EateryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        // 1. Lấy danh sách Quán ăn cho Admin
        public async Task<IEnumerable<EateryAdminOwnerResponseDto>> GetAllForAdminAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                SELECT 
                    id AS Id, name AS Name, address AS Address, 
                    owner_id AS OwnerId, category_id AS CategoryId, 
                    price_range AS PriceRange, description AS Description, 
                    rating AS Rating, latitude AS Latitude, longitude AS Longitude, 
                    audio_file_path AS AudioFilePath, image_path AS ImagePath, 
                    open_time AS OpenTime, close_time AS CloseTime, 
                    is_approved AS IsApproved, created_at AS CreatedAt
                FROM eateries
                ORDER BY id DESC";
            
            return await connection.QueryAsync<EateryAdminOwnerResponseDto>(sql);
        }

        // 2. Quyền lực Admin: Duyệt hoặc Khóa quán ăn
        public async Task<bool> ChangeApprovalStatusAsync(int id, bool isApproved)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "UPDATE eateries SET is_approved = @IsApproved WHERE id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { IsApproved = isApproved, Id = id });
            return affectedRows > 0;
        }

        // 3. Tạo quán ăn 
        public async Task<int> CreateEateryAsync(int ownerId, CreateEateryRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                INSERT INTO eateries 
                (name, address, owner_id, category_id, price_range, description, latitude, longitude, audio_file_path, image_path, open_time, close_time) 
                VALUES 
                (@Name, @Address, @OwnerId, @CategoryId, @PriceRange, @Description, @Latitude, @Longitude, @AudioFilePath, @ImagePath, @OpenTime, @CloseTime);
                SELECT last_insert_rowid();";
            
            return await connection.ExecuteScalarAsync<int>(sql, new {
                request.Name, request.Address, OwnerId = ownerId, request.CategoryId, request.PriceRange, 
                request.Description, request.Latitude, request.Longitude, request.AudioFilePath, 
                request.ImagePath, request.OpenTime, request.CloseTime
            });
        }
    }
}