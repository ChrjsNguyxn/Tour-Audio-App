namespace backend.DTOs;

/// <summary>
/// - Tương tự với Create
/// </summary>
public class VendorUpdateDto
{
    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string? PriceRange { get; set; }

    public string? DescriptionVi { get; set; }

    public string? DescriptionEn { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? ImagePath { get; set; }

    public string? OpenTime { get; set; }

    public string? CloseTime { get; set; }
}