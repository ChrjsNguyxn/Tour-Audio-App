using FoodMapAPI.DTOs;
using FoodMapAPI.Models;
using FoodMapAPI.Repository;

namespace FoodMapAPI.Service
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepo;

        public OwnerService(IOwnerRepository ownerRepo)
        {
            _ownerRepo = ownerRepo;
        }

        public async Task<OwnerResponseDto> RegisterAsync(RegisterOwnerDto dto)
        {
            // ----- Logic nghiệp vụ: validate -----
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email không được để trống.");

            if (dto.Password.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.");

            var existing = await _ownerRepo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new ArgumentException("Email đã được sử dụng.");

            // ----- Tạo Owner mới, mặc định chờ duyệt -----
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

            return MapToDto(created);
        }

        public async Task<object> LoginAsync(LoginOwnerDto dto)
        {
            var owner = await _ownerRepo.GetByEmailAsync(dto.Email);

            if (owner == null || !BCrypt.Net.BCrypt.Verify(dto.Password, owner.PasswordHash))
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng.");

            if (owner.Status == "Pending")
                throw new UnauthorizedAccessException("Tài khoản đang chờ Admin duyệt.");

            if (owner.Status == "Locked")
                throw new UnauthorizedAccessException("Tài khoản đã bị khóa.");

            return new { message = "Đăng nhập thành công", ownerId = owner.Id, name = owner.FullName };
        }

        public async Task<OwnerResponseDto?> GetByIdAsync(int id)
        {
            var owner = await _ownerRepo.GetByIdAsync(id);
            return owner == null ? null : MapToDto(owner);
        }

        // ----- Hàm dùng chung: map Model -> DTO -----
        private static OwnerResponseDto MapToDto(Owner owner)
        {
            return new OwnerResponseDto
            {
                Id = owner.Id,
                FullName = owner.FullName,
                Email = owner.Email,
                PhoneNumber = owner.PhoneNumber,
                Status = owner.Status,
                CreatedAt = owner.CreatedAt
            };
        }
    }
}