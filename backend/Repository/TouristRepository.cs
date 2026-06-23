using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using backend.DTOs.TouristDTO;
using Microsoft.Extensions.Configuration;

namespace backend.Repository
{
    public class TouristRepository
    {
        private readonly string _connectionString;

        public TouristRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        // 1. Khách xem danh sách quán ăn (CHỈ QUÁN ĐÃ DUYỆT: is_approved = 1)
        public async Task<IEnumerable<TouristEateryResponseDto>> GetApprovedEateriesAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                SELECT 
                    id AS Id, name AS Name, address AS Address, 
                    price_range AS PriceRange, description AS Description, 
                    rating AS Rating, latitude AS Latitude, longitude AS Longitude, 
                    audio_file_path AS AudioFilePath, image_path AS ImagePath, 
                    open_time AS OpenTime, close_time AS CloseTime
                FROM eateries
                WHERE is_approved = 1
                ORDER BY id DESC";
            
            return await connection.QueryAsync<TouristEateryResponseDto>(sql);
        }

        // 2. Khách xem chi tiết 1 quán ăn
        public async Task<TouristEateryResponseDto?> GetEateryDetailAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                SELECT 
                    id AS Id, name AS Name, address AS Address, 
                    price_range AS PriceRange, description AS Description, 
                    rating AS Rating, latitude AS Latitude, longitude AS Longitude, 
                    audio_file_path AS AudioFilePath, image_path AS ImagePath, 
                    open_time AS OpenTime, close_time AS CloseTime
                FROM eateries
                WHERE id = @Id AND is_approved = 1";
            
            return await connection.QuerySingleOrDefaultAsync<TouristEateryResponseDto>(sql, new { Id = id });
        }
    }
}