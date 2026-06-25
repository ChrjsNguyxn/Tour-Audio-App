namespace FoodMapAPI.Models
{
    public class ViewLog
    {
        public int Id { get; set; }
        public int EateryId { get; set; }
        public Eatery? Eatery { get; set; }
        public string ActionType { get; set; } = "view"; // view, listen_audio, favorite
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}