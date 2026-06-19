namespace backend.Models;

public class MenuItem
{
    public int Id { get; set; }

    public int VendorId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? ImagePath { get; set; }

    public int Price { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}