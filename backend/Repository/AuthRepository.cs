using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using backend.DTOs.AuthDTO;

namespace backend.Repository
{
    public class AuthRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public AuthRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Database/foodtour.db";
        }

        // ==========================================
        // 1. HÀM ĐĂNG KÝ (TẠO TÀI KHOẢN MỚI)
        // ==========================================
        public async Task<int> RegisterAsync(RegisterRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            
            // Lưu ý: Tên bảng là 'users'. 
            var sql = @"
                INSERT INTO users (username, password, email, full_name, role, is_active) 
                VALUES (@Username, @Password, @Email, @FullName, @Role, 1);
                SELECT last_insert_rowid();";
                
            return await connection.ExecuteScalarAsync<int>(sql, new {
                request.Username,
                request.Password, // Lưu ý: Mật khẩu đang được lưu dạng text thuần.
                request.Email,
                request.FullName,
                request.Role
            });
        }

        // ==========================================
        // 2. HÀM KIỂM TRA TÀI KHOẢN (Không tạo token)
        // ==========================================
        public async Task<dynamic?> FindUserByUsernameAndPasswordAsync(LoginRequestDto request)
        {
            using var connection = new SqliteConnection(_connectionString);
            
            // Tìm user theo username
            var sql = "SELECT id, username, password, role, is_active as IsActive FROM users WHERE username = @Username";
            var user = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Username = request.Username });

            // 1. Không tìm thấy tài khoản
            if (user == null) return null; 

            // 2. So sánh mật khẩu (Đang để check text thuần, ép kiểu để chắc chắn)
            if (Convert.ToString(user.password) != request.Password) return null; 

            // 3. Đúng Pass thì trả về thông tin
            return user;
        }

        public async Task<AuthResponseDto?> AuthenticateAsync(string username, string password)
        {
            using var connection = new SqliteConnection(_connectionString);
            
            // Tìm User trong DB (Thực tế sau này sẽ xài Hash Password, tạm thời query thẳng cho bạn dễ test)
            var sql = "SELECT id AS Id, username AS Username, role AS Role, is_active AS IsActive FROM users WHERE username = @Username AND password = @Password";
            var user = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { Username = username, Password = password });

            // Nếu sai tài khoản hoặc bị Admin khóa mõm (IsActive = 0)
            if (user == null || user.IsActive == 0)
            {
                return null; 
            }

            // ĐÚC TOKEN
            var tokenHandler = new JwtSecurityTokenHandler();
            // Lấy mã bí mật (Ở đây mình hardcode luôn một chuỗi siêu dài để bạn đỡ phải cấu hình appsettings.json lằng nhằng)
            var key = Encoding.ASCII.GetBytes("KhoaBiMatCucKyDaiVaNguyHiemCuaDuAnFoodTourQuan4123456789");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token xài được 7 ngày
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Id = (int)user.Id,
                Role = user.Role
            };
        }

        // ==========================================
        // 3. HÀM LẤY DANH SÁCH NGƯỜI DÙNG
        // ==========================================
        public async Task<IEnumerable<dynamic>> GetAllUsersAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = "SELECT id, username, email, full_name AS fullName, role, is_active as isActive FROM users ORDER BY id DESC";
            return await connection.QueryAsync<dynamic>(sql);
        }
    }
}