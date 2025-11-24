namespace ShiemiApi.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDesc { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Profile { get; set; }

    public int UserId { get; set; }
}