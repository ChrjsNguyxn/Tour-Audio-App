using Microsoft.EntityFrameworkCore;
using Quan4.AdminApi.Data;
using Quan4.AdminApi.DTOs;
using Quan4.AdminApi.Models;

namespace Quan4.AdminApi.Services
{
    public class VendorService
    {
        private readonly AppDbContext _context;

        public VendorService(AppDbContext context)
        {
            _context = context;
        }

        // 1. TẠO QUÁN ĂN
        public async Task<(bool Success, string Message)> CreateVendorAsync(CreateVendorRequestDto request, int ownerId)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
            if (!categoryExists) return (false, "Danh mục (Category) không tồn tại!");

            var vendor = new Vendor
            {
                Name = request.Name,
                Address = request.Address,
                CategoryId = request.CategoryId,
                PriceRange = request.PriceRange,
                Description = request.Description,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime,
                OwnerId = ownerId,
                IsApproved = false
            };

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return (true, $"Đã gửi yêu cầu tạo quán '{request.Name}' thành công. Chờ Admin duyệt!");
        }

        // 2. LẤY DANH SÁCH CHƯA DUYỆT
        public async Task<List<VendorResponseDto>> GetUnapprovedVendorsAsync()
        {
            return await _context.Vendors
                .Where(v => v.IsApproved == false)
                .Include(v => v.Category)
                .Include(v => v.Owner)
                .Select(v => new VendorResponseDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Address = v.Address,
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
        }

        // 3. DUYỆT QUÁN ĂN
        public async Task<(bool Success, string Message)> ApproveVendorAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return (false, "Không tìm thấy quán ăn này!");

            vendor.IsApproved = true;
            await _context.SaveChangesAsync();

            return (true, $"Đã duyệt quán '{vendor.Name}' hiển thị lên ứng dụng!");
        }

        // 4. XÓA QUÁN ĂN
        public async Task<(bool Success, string Message)> DeleteVendorAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return (false, "Không tìm thấy quán ăn!");
            
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            
            return (true, "Xóa thành công!");
        }

        // 5. LẤY DANH SÁCH ĐÃ DUYỆT
        public async Task<List<VendorResponseDto>> GetAllApprovedVendorsAsync()
        {
            return await _context.Vendors
                .Where(v => v.IsApproved == true)
                .Include(v => v.Category)
                .Select(v => new VendorResponseDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Address = v.Address,
                    PriceRange = v.PriceRange,
                    Description = v.Description,
                    CategoryName = v.Category != null ? v.Category.Name : "Chưa phân loại"
                })
                .ToListAsync();
        }
    }
}