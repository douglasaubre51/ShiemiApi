namespace ShiemiApi.Models;

public class Dev
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public string Advert { get; set; } = string.Empty;
    public string ShortDesc { get; set; } = string.Empty;
    public decimal StartingPrice { get; set; }
}