using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using backend.DTOs.ReviewDTO;
using Microsoft.Extensions.Configuration;

namespace backend.Repository
{
    public class ReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        // 1. Lấy tất cả đánh giá của 1 quán ăn
        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByEateryIdAsync(int eateryId)
        {
            using var connection = new SqliteConnection(_connectionString);
            // Kết hợp (JOIN) bảng reviews và bảng users để lấy được cái Username ra hiển thị
            var sql = @"
                SELECT 
                    r.id AS Id, r.eatery_id AS EateryId, r.user_id AS UserId, 
                    u.username AS Username, r.rating AS Rating, 
                    r.comment AS Comment, r.created_at AS CreatedAt
                FROM reviews r
                JOIN users u ON r.user_id = u.id
                WHERE r.eatery_id = @EateryId
                ORDER BY r.id DESC";
            
            return await connection.QueryAsync<ReviewResponseDto>(sql, new { EateryId = eateryId });
        }

        // 2. Thêm đánh giá mới
        public async Task<int> CreateReviewAsync(int eateryId, int userId, CreateReviewRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                INSERT INTO reviews (eatery_id, user_id, rating, comment) 
                VALUES (@EateryId, @UserId, @Rating, @Comment);
                SELECT last_insert_rowid();";
            
            return await connection.ExecuteScalarAsync<int>(sql, new {
                EateryId = eateryId, UserId = userId, 
                request.Rating, request.Comment
            });
        }
    }
}