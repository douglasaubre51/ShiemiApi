namespace ShiemiApi.Models;

public class Message
{
    public int Id { get; set; }

    public string? Text { get; set; }
    public string? Voice { get; set; }
    public string? Video { get; set; }
    public string? Photo { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = new();
    public Channel? Channel { get; set; }
    public Room? Room { get; set; }
}