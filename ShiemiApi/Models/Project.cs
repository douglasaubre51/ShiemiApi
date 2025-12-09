namespace ShiemiApi.Models;

public class Project
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public Channel? Channel { get; set; }
    public List<Room> PrivateRooms { get; set; } = [];
    public List<int> UserList { get; set; } = [];
    public List<int> BlockList { get; set; } = [];

    public string Title { get; set; } = string.Empty;
    public string ShortDesc { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Profile { get; set; } = string.Empty;
    public DateOnly CreatedAt { get; set; }
    public decimal Cost { get; set; }
}