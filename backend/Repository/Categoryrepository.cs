using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;
using Backend.DTOs.CategoryDTO;

namespace backend.Repository
{
    public class CategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                SELECT 
                    id AS Id, name AS Name, description AS Description, created_at AS CreatedAt 
                FROM categories
                ORDER BY id DESC";
            return await connection.QueryAsync<CategoryResponseDto>(sql);
        }

        public async Task<int> CreateCategoryAsync(CreateCategoryRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                INSERT INTO categories (name, description) 
                VALUES (@Name, @Description);
                SELECT last_insert_rowid();";
            return await connection.ExecuteScalarAsync<int>(sql, request);
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                UPDATE categories 
                SET name = @Name, description = @Description 
                WHERE id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { request.Name, request.Description, Id = id });
            return affectedRows > 0;
        }
    }
}