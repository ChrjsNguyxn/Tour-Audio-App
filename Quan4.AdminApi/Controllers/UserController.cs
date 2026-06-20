using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quan4.AdminApi.Data;
using Quan4.AdminApi.DTOs;
using Quan4.AdminApi.Models;

namespace Quan4.AdminApi.Controllers
{
    [Route("api/v1/admin/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH NGƯỜI DÙNG (GET) - Đã cập nhật theo cấu trúc mới
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role,
                    FullName = u.FullName,
                    Email = u.Email,
                    IsActive = u.IsActive
                })
                .ToListAsync();

            return Ok(users);
        }

        // 2. TẠO TÀI KHOẢN MỚI (POST)
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("Tên đăng nhập này đã tồn tại trong hệ thống!");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã cấp tài khoản {request.Username} với quyền {request.Role} thành công!" });
        }
    }
}