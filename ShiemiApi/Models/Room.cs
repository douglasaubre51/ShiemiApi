namespace ShiemiApi.Models;

public class Room
{
    public int Id { get; set; }
    public User Owner { get; set; } = new();
    public User Tenant { get; set; } = new();

    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    public int? DevId { get; set; }
    public Dev? Dev { get; set; }

    public List<Message>? Messages { get; set; } = [];
    public RoomTypes RoomType { get; set; } = RoomTypes.PRIVATE;
}
