using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;
using backend.DTOs;
using backend.DTOs.EateryDTO;

namespace backend.Repository
{
    public class EateryRepository
    {
        private readonly string _connectionString;

        public EateryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        // 1. Lấy danh sách Quán ăn cho Admin (Đã lấy thêm cột status và action_reason)
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
                    is_approved AS IsApproved, created_at AS CreatedAt,
                    status AS Status, action_reason AS ActionReason
                FROM eateries
                ORDER BY id DESC";
            
            return await connection.QueryAsync<EateryAdminOwnerResponseDto>(sql);
        }

        // 2. Hàm tối thượng: Thay đổi trạng thái (Duyệt / Khóa / Xóa mềm) kèm theo lý do
        public async Task<bool> ChangeEateryStatusAsync(int id, string status, string reason)
        {
            using var connection = new SqliteConnection(_connectionString);
            // Vừa cập nhật cột status mới cho Admin quản lý, vừa cập nhật cột is_approved cũ 
            // để đảm bảo các API cũ của App Mobile quét QR không bị lỗi logic (Duyệt thì bằng 1, ngược lại bằng 0)
            var sql = @"
                UPDATE eateries 
                SET status = @Status, 
                    action_reason = @Reason,
                    is_approved = CASE WHEN @Status = 'approved' THEN 1 ELSE 0 END
                WHERE id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new { Status = status, Reason = reason, Id = id });
            return affectedRows > 0;
        }

        // 3. Tạo quán ăn mới
        public async Task<int> CreateEateryAsync(int ownerId, CreateEateryRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                INSERT INTO eateries 
                (name, address, owner_id, category_id, price_range, description, latitude, longitude, audio_file_path, image_path, open_time, close_time, status, is_approved) 
                VALUES 
                (@Name, @Address, @OwnerId, @CategoryId, @PriceRange, @Description, @Latitude, @Longitude, @AudioFilePath, @ImagePath, @OpenTime, @CloseTime, 'pending', 0);
                SELECT last_insert_rowid();"; 
                // Ở dòng VALUES trên, thay 'approved', 1 thành 'pending', 0
            
            return await connection.ExecuteScalarAsync<int>(sql, new {
                request.Name, request.Address, OwnerId = ownerId, request.CategoryId, request.PriceRange, 
                request.Description, request.Latitude, request.Longitude, request.AudioFilePath, 
                request.ImagePath, request.OpenTime, request.CloseTime
            });
        }

        // 4. Cập nhật thông tin quán (Sửa tên, địa chỉ, mô tả)
        /*
        public async Task<int> UpdateEateryAsync(int id, UpdateEateryAdminDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                UPDATE eateries 
                SET name = @Name, address = @Address, description = @Description 
                WHERE id = @Id;";
            return await connection.ExecuteAsync(sql, new { request.Name, request.Address, request.Description, Id = id });
        }
        */
        public async Task<int> UpdateEateryAsync(int id, UpdateEateryAdminDto request)
        {
            using var connection = new SqliteConnection(_connectionString);

            var sql = @"
                UPDATE eateries
                SET
                    name = @Name,
                    address = @Address,
                    description = @Description,
                    category_id = @CategoryId,
                    latitude = @Latitude,
                    longitude = @Longitude,
                    price_range = @PriceRange,
                    image_path = @ImagePath,
                    audio_file_path = @AudioFilePath,
                    open_time = @OpenTime,
                    close_time = @CloseTime
                WHERE id = @Id;";

            return await connection.ExecuteAsync(sql, new
            {
                request.Name,
                request.Address,
                request.Description,
                request.CategoryId,
                request.Latitude,
                request.Longitude,
                request.PriceRange,
                request.ImagePath,
                request.AudioFilePath,
                request.OpenTime,
                request.CloseTime,
                Id = id
            });
        }
    }
}