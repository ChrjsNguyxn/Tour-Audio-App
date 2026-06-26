namespace backend.DTOs.StatsDTO
{
    public class DailyStatPointDto
    {
        public string Date { get; set; } = string.Empty; // "dd/MM"
        public int Count { get; set; }
    }

    public class EateryStatsResponseDto
    {
        public int TotalViews { get; set; }
        public int TotalListens { get; set; }
        public int TotalFavorites { get; set; }
        public List<DailyStatPointDto> Last7Days { get; set; } = new();
    }
}
