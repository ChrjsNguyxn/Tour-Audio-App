using Microsoft.AspNetCore.Mvc;
using FoodMapAPI.DTOs;
using FoodMapAPI.Models;
using FoodMapAPI.Repository;

namespace FoodMapAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepo;

        public OwnersController(IOwnerRepository ownerRepo)
        {
            _ownerRepo = ownerRepo;
        }

        // POST: api/owners/register
        [HttpPost("register")]
        public async Task<ActionResult<OwnerResponseDto>> Register(RegisterOwnerDto dto)
        {
            var existing = await _ownerRepo.GetByEmailAsync(dto.Email);
            if (existing != null)
                return BadRequest("Email đã được sử dụng.");

            var owner = new Owner
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            var created = await _ownerRepo.CreateAsync(owner);

            return Ok(new OwnerResponseDto
            {
                Id = created.Id,
                FullName = created.FullName,
                Email = created.Email,
                PhoneNumber = created.PhoneNumber,
                Status = created.Status,
                CreatedAt = created.CreatedAt
            });
        }

        // POST: api/owners/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginOwnerDto dto)
        {
            var owner = await _ownerRepo.GetByEmailAsync(dto.Email);

            if (owner == null || !BCrypt.Net.BCrypt.Verify(dto.Password, owner.PasswordHash))
                return Unauthorized("Email hoặc mật khẩu không đúng.");

            if (owner.Status == "Pending")
                return Unauthorized("Tài khoản đang chờ Admin duyệt.");

            if (owner.Status == "Locked")
                return Unauthorized("Tài khoản đã bị khóa.");

            return Ok(new { message = "Đăng nhập thành công", ownerId = owner.Id, name = owner.FullName });
        }

        // GET: api/owners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerResponseDto>> GetOwner(int id)
        {
            var owner = await _ownerRepo.GetByIdAsync(id);
            if (owner == null) return NotFound();

            return Ok(new OwnerResponseDto
            {
                Id = owner.Id,
                FullName = owner.FullName,
                Email = owner.Email,
                PhoneNumber = owner.PhoneNumber,
                Status = owner.Status,
                CreatedAt = owner.CreatedAt
            });
        }
    }
}