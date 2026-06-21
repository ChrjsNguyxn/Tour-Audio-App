using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodMapAPI.Data;
using FoodMapAPI.Models;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OwnersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/owners/register
        [HttpPost("register")]
        public async Task<ActionResult<Owner>> Register(Owner owner)
        {
            // Kiểm tra email đã tồn tại chưa
            var existing = await _context.Owners
                .FirstOrDefaultAsync(o => o.Email == owner.Email);
            if (existing != null)
                return BadRequest("Email đã được sử dụng.");

            // Hash password đơn giản (tạm thời)
            owner.PasswordHash = BCrypt.Net.BCrypt.HashPassword(owner.PasswordHash);
            owner.CreatedAt = DateTime.Now;

            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOwner), new { id = owner.Id }, owner);
        }

        // POST: api/owners/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var owner = await _context.Owners
                .FirstOrDefaultAsync(o => o.Email == request.Email);

            if (owner == null || !BCrypt.Net.BCrypt.Verify(request.Password, owner.PasswordHash))
                return Unauthorized("Email hoặc mật khẩu không đúng.");

            return Ok(new { message = "Đăng nhập thành công", ownerId = owner.Id, name = owner.FullName });
        }

        // GET: api/owners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Owner>> GetOwner(int id)
        {
            var owner = await _context.Owners
                .Include(o => o.Shops)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (owner == null) return NotFound();
            return owner;
        }
    }

    // Class phụ cho Login
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}