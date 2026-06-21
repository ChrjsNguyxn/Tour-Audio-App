namespace backend.Models;

public class MenuItem
{
    public int Id { get; set; }

    public int EateryId { get; set; }

    public string Name { get; set; } = "";

    public string? ImagePath { get; set; }

    public int Price { get; set; }

    public string? Description { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }
}