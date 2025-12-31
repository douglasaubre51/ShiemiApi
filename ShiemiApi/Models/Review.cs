namespace ShiemiApi.Models;

public class Review
{
    public int Id { get; set; }
    public User? User { get; set; }
    public Project? Project { get; set; }

    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
