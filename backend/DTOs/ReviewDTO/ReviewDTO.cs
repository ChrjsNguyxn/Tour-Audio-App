namespace backend.DTOs.ReviewDTO
{
    // Khuôn mẫu khi Khách gửi đánh giá lên
    public class CreateReviewRequestDto
    {
        public int Rating { get; set; } // Từ 1 đến 5 sao
        public string? Comment { get; set; }
    }

    // Khuôn mẫu trả về để hiển thị danh sách bình luận
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int EateryId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty; // Hiện tên người đánh giá
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string? CreatedAt { get; set; }
    }
}