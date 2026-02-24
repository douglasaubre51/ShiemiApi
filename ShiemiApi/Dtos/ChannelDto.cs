namespace ShiemiApi.Dtos;

public class ChannelDto
{
    public int Id;
    public int ProjectId;
    public string Title = string.Empty;
    public string Profile = string.Empty;
    public string PinnedMessage = string.Empty;
    public List<int> Messages = [];
    public ProjectDto? CurrentProject;
}

public record AddChannelDto(
    int ProjectId,
    int UserId
);

public record GetAllChannelDto(
    int Id,
    int ProjectId,
    int UserId,
    string Title,
    string UserProfile
        );
