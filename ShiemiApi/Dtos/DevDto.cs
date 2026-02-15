namespace ShiemiApi.Dtos;

public class DevDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string Advert { get; set; } = string.Empty;
    public string ShortDesc { get; set; } = string.Empty;
    public decimal StartingPrice { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Profile { get; set; } = string.Empty;
}

public class SearchDevDto
{
    public int DevId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
}

