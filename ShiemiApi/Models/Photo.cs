namespace ShiemiApi.Models;

public class Photo
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? DevId { get; set; }
    public User? User { get; set; }
    public Dev? Dev { get; set; }

    public string PublicId { get; set; } = string.Empty;
    public string URL { get; set; } = string.Empty;
}
