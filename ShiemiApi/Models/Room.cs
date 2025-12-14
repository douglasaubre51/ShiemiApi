namespace ShiemiApi.Models;

public class Room
{
    public int Id { get; set; }
    public User Owner { get; set; } = new();
    public User Tenant { get; set; } = new();
    public int ProjectId { get; set; }
    public Project Project { get; set; } = new();

    public List<Message> Messages { get; set; } = [];
}