namespace backend.DTOs.DashboardDTO
{
    public class DashboardResponseDto
    {
        public int TotalUsers { get; set; }
        public int TotalEateries { get; set; }
        public int TotalPendingEateries { get; set; } // Quán chờ duyệt
        public int TotalMenuItems { get; set; }
        public int TotalReviews { get; set; }
    }
}