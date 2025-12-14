namespace ShiemiApi.Dtos;

public class ReviewDtos
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProjectId { get; set; }

    public string Text { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Profile { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public record CreateReviewDto(
     int UserId,
     int ProjectId,
     string Text,
     DateTime CreatedAt
);