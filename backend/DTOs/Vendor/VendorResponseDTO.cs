namespace backend.DTOs;

/// <summary>
/// - Bỏ các trường:
/// OwnerId
/// CreatedAt
/// IsApproved
/// </summary>
public class VendorResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? PriceRange { get; set; }

    public string? DescriptionVi { get; set; }

    public string? DescriptionEn { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? AudioFilePath { get; set; }

    public string? ImagePath { get; set; }

    public string? OpenTime { get; set; }

    public string? CloseTime { get; set; }

    public int CategoryId { get; set; }
}