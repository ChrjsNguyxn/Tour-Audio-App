using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;
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
                UserId = (int)user.Id,
                Role = user.Role
            };
        }
    }
}