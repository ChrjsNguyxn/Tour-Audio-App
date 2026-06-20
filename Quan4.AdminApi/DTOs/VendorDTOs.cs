namespace Quan4.AdminApi.DTOs
{
    // Khuôn dùng để gửi dữ liệu tạo quán ăn mới lên Server
    public class CreateVendorRequest
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string PriceRange { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string OpenTime { get; set; } = string.Empty;
        public string CloseTime { get; set; } = string.Empty;
    }

    // Khuôn dùng để trả về thông tin quán ăn ra màn hình App/Web
    public class VendorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty; // Tên danh mục (VD: Bún bò)
        public string PriceRange { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AudioFilePath { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string OpenTime { get; set; } = string.Empty;
        public string CloseTime { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}