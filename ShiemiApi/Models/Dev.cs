namespace ShiemiApi.Models;

public class Dev
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public List<Room> DevRooms { get; set; } = [];

    public Photo? Advert { get; set; }
    public string ShortDesc { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal StartingPrice { get; set; }
}
