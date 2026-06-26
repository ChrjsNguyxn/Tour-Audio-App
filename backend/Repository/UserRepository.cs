using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using backend.DTOs.UserDTO;
using Microsoft.Extensions.Configuration;

namespace backend.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        // 1. Lấy tất cả (READ)
        public async Task<IEnumerable<dynamic>> GetAllUsersAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "SELECT id AS Id, username AS Username, full_name AS FullName, email AS Email, role AS Role, is_active AS IsActive FROM users ORDER BY id DESC";
            return await connection.QueryAsync(sql);
        }

        // 2. Thêm mới (CREATE)
        public async Task<int> CreateUserAsync(CreateUserAdminDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            // Lưu ý: Ở dự án thật Password sẽ được mã hóa (Hash). Để test nhanh, ta lưu thẳng.
            var sql = @"
                INSERT INTO users (username, password_hash, full_name, email, role, is_active) 
                VALUES (@Username, @Password, @FullName, @Email, @Role, 1);
                SELECT last_insert_rowid();";
            return await connection.ExecuteScalarAsync<int>(sql, request);
        }

        // 3. Sửa thông tin (UPDATE)
        public async Task<int> UpdateUserAsync(int id, UpdateUserAdminDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                UPDATE users 
                SET full_name = @FullName, email = @Email, role = @Role
                WHERE id = @Id;";
            return await connection.ExecuteAsync(sql, new { request.FullName, request.Email, request.Role, Id = id });
        }

        // 4. Đổi trạng thái Khóa / Mở (UPDATE STATUS)
        public async Task<int> ToggleStatusAsync(int id, bool isActive)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "UPDATE users SET is_active = @IsActive WHERE id = @Id;";
            return await connection.ExecuteAsync(sql, new { IsActive = isActive ? 1 : 0, Id = id });
        }

        // 5. Xóa vĩnh viễn (DELETE)
        public async Task<int> DeleteUserAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "DELETE FROM users WHERE id = @Id;";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}