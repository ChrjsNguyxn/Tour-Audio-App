namespace backend.DTOs;

/// <summary>
/// DTO chứa món ăn của một POI(eatery). Sẽ được dùng trong POI ở dạng mảng
/// </summary>
public class POIMenuDTO
{
    public int Id { get; set; }

    public int EateryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? ImagePath { get; set; }

    public int Price { get; set; }

    public string? Description { get; set; }
}


/// <summary>
/// DTO POI chứa thông tin của Eatery + tên danh mục + mảng các món ăn(menu_item)
/// </summary>
public class POIResponseDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int OwnerId { get; set; }

    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? PriceRange { get; set; }

    public string? Description { get; set; }

    public double Rating { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? AudioFilePath { get; set; }

    public string? ImagePath { get; set; }

    public string? OpenTime { get; set; }

    public string? CloseTime { get; set; }

    public List<POIMenuDTO> MenuItems { get; set; } = new();
}