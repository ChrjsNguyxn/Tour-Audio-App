using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    // [Authorize] // Tạm thời tắt khóa để bạn dễ test postman, test xong mở comment ra nhé
    public class UploadController : ControllerBase
    {
        // 1. Làn nhận Hình ảnh
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0) 
                return BadRequest(new { message = "Không có file nào được chọn." });
            
            // Tạo thư mục wwwroot/uploads/images nếu chưa tồn tại
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            // Tạo tên file mới (tránh trùng tên)
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            // Lưu file vào ổ cứng
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { message = "Tải ảnh lên thành công!", url = $"/uploads/images/{fileName}" });
        }

        // 2. Làn nhận Âm thanh (Audio)
        [HttpPost("audio")]
        public async Task<IActionResult> UploadAudio(IFormFile file)
        {
            if (file == null || file.Length == 0) 
                return BadRequest(new { message = "Không có file nào được chọn." });
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "audio");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { message = "Tải audio lên thành công!", url = $"/uploads/audio/{fileName}" });
        }
    }
}