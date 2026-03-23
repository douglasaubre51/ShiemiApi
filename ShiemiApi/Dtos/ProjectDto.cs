namespace ShiemiApi.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int ChannelId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string ShortDesc { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UserProfilePhoto { get; set; } = string.Empty;
}

public record EditProjectDto(
         int Id,
         string Title,
         string ShortDesc,
         string Description,
         List<string> Tags
        );
