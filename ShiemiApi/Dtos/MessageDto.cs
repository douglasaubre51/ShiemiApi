namespace ShiemiApi.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    
    public string? Text { get; set; }
    public string? Voice { get; set; }
    public string? Video { get; set; }
    public string? Photo { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int UserId { get; set; }
    public int ChannelId { get; set; }
    public int RoomId { get; set; }
}