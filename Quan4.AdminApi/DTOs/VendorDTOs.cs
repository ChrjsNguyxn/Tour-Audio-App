namespace Quan4.AdminApi.DTOs
{
    // 1. Gói hàng trả về cho React (Response)
    public class VendorResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty; 
        public string OwnerName { get; set; } = string.Empty;
        public string? PriceRange { get; set; }
        public string? Description { get; set; }
        public double Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? ImagePath { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
        public bool IsApproved { get; set; }
    }

    // 2. Gói hàng nhận từ React gửi lên (Request)
    public class CreateVendorRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string? PriceRange { get; set; }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
    }
}