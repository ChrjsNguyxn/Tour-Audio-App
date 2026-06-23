using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;

using Microsoft.Extensions.Configuration;
using Backend.DTOs.UserDTO;

namespace backend.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        // 1. Lấy danh sách tất cả User (Dành cho Admin)
        public async Task<IEnumerable<UserDetailResponseDto>> GetAllUsersForAdminAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                SELECT 
                    id AS Id, 
                    username AS Username, 
                    full_name AS FullName, 
                    email AS Email, 
                    avatar_url AS AvatarUrl, 
                    role AS Role, 
                    is_active AS IsActive, 
                    created_at AS CreatedAt 
                FROM users
                ORDER BY id DESC";
            
            return await connection.QueryAsync<UserDetailResponseDto>(sql);
        }

        // 2. Quyền lực Admin: Khóa / Mở khóa tài khoản User
        public async Task<bool> ChangeUserStatusAsync(int id, bool isActive)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "UPDATE users SET is_active = @IsActive WHERE id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { IsActive = isActive, Id = id });
            return affectedRows > 0;
        }
    }
}