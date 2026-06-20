using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quan4.AdminApi.Data;
using Quan4.AdminApi.DTOs;
using Quan4.AdminApi.Models;
using System.Security.Claims;

namespace Quan4.AdminApi.Controllers
{
    [Route("api/v1/vendors")]
    [ApiController]
    [Authorize] // Bắt buộc đăng nhập mới được xài các API này
    public class VendorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendorController(AppDbContext context)
        {
            _context = context;
        }

        // 1. API TẠO QUÁN ĂN MỚI (Dành cho Minh tham khảo hoặc chủ quán tạo)
        [HttpPost]
        public async Task<IActionResult> CreateVendor(CreateVendorRequest request)
        {
            // Lấy ID của người đang đăng nhập từ Token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();
            int currentUserId = int.Parse(userIdClaim);

            // Kiểm tra danh mục có tồn tại không
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
            if (!categoryExists) return BadRequest("Danh mục (Category) không tồn tại!");

            var vendor = new Vendor
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                PriceRange = request.PriceRange,
                Description = request.Description,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime,
                OwnerId = currentUserId,
                IsApproved = false // Mặc định tạo xong phải đợi Admin duyệt
            };

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã gửi yêu cầu tạo quán '{request.Name}' thành công. Chờ Admin duyệt!" });
        }

        // 2. API LẤY DANH SÁCH CÁC QUÁN CHƯA DUYỆT (Quyền lực của Admin Thụy)
        [HttpGet("unapproved")]
        public async Task<IActionResult> GetUnapprovedVendors()
        {
            var vendors = await _context.Vendors
                .Where(v => v.IsApproved == false)
                .Include(v => v.Category)
                .Include(v => v.Owner)
                .Select(v => new VendorResponse
                {
                    Id = v.Id,
                    Name = v.Name,
                    CategoryName = v.Category != null ? v.Category.Name : "Chưa phân loại",
                    PriceRange = v.PriceRange,
                    Description = v.Description,
                    Latitude = v.Latitude,
                    Longitude = v.Longitude,
                    OpenTime = v.OpenTime,
                    CloseTime = v.CloseTime,
                    IsApproved = v.IsApproved,
                    OwnerName = v.Owner != null ? v.Owner.FullName : "Ẩn danh"
                })
                .ToListAsync();

            return Ok(vendors);
        }

        // 3. API DUYỆT QUÁN ĂN (Quyền lực của Admin Thụy)
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return NotFound("Không tìm thấy quán ăn này!");

            vendor.IsApproved = true; // Chuyển trạng thái thành Đã Duyệt
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã duyệt quán '{vendor.Name}' hiển thị lên ứng dụng!" });
        }

        // 4. API XÓA QUÁN ĂN
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return NotFound("Không tìm thấy quán ăn!");
            
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa thành công!" });
        }

        // 5. API LẤY DANH SÁCH QUÁN ĂN (Dành cho Khách du lịch - Không cần Token)
        [HttpGet]
        [AllowAnonymous] // Bỏ qua kiểm tra bảo mật cho API này
        public async Task<IActionResult> GetAllApprovedVendors()
        {
            var vendors = await _context.Vendors
                // Chỉ lấy những quán đã được Admin Thụy duyệt
                .Where(v => v.IsApproved == true)
                .Select(v => new 
                { 
                    v.Id, 
                    v.Name, 
                    v.PriceRange, 
                    v.Description,
                    CategoryName = v.Category != null ? v.Category.Name : "Chưa phân loại"
                })
                .ToListAsync();

            return Ok(vendors);
        }
    }
}