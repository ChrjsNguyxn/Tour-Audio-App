namespace backend.Models;

public class Eatery
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Address { get; set; } = "";

    public int OwnerId { get; set; }

    public int CategoryId { get; set; }

    public string? PriceRange { get; set; }

    public string? Description { get; set; }

    public double Rating { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string? AudioFilePath { get; set; }

    public string? ImagePath { get; set; }

    public string? OpenTime { get; set; }

    public string? CloseTime { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }
}