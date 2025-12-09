namespace ShiemiApi.Models;

public class Message
{
    public int Id { get; set; }
    public User? User { get; set; }
    public Channel? Channel { get; set; }
    public Room? Room { get; set; }

    public string Text { get; set; } = string.Empty;
    public string Voice { get; set; } = string.Empty;
    public string Video { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}