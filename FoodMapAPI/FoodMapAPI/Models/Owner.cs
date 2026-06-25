namespace FoodMapAPI.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Một owner có thể có nhiều quán
        public List<Eatery> Shops { get; set; } = new List<Eatery>();
        public string Status { get; set; } = "Pending";
    }
}