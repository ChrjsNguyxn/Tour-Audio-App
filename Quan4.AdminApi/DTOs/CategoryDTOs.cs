namespace Quan4.AdminApi.DTOs
{
    // Khuôn dùng để Admin nhập dữ liệu tạo danh mục mới
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    // Khuôn dùng để xuất danh sách danh mục ra ngoài
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}