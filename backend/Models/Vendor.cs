namespace backend.Models;

public class Vendor
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string? PriceRange { get; set; }

    /// <summary>
    /// Vietnamese description shown to local users.
    /// </summary>
    public string? DescriptionVi { get; set; }

    /// <summary>
    /// English description shown to international users.
    /// </summary>
    public string? DescriptionEn { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    /// <summary>
    /// Path to generated audio file.
    /// </summary>
    public string? AudioFilePath { get; set; }

    public string? ImagePath { get; set; }

    public string? OpenTime { get; set; }

    public string? CloseTime { get; set; }

    public int OwnerId { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }
}