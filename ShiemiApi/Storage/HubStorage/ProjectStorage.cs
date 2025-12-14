using ShiemiApi.Dtos.HubDtos;

namespace ShiemiApi.Storage.HubStorage;

public class ProjectStorage
{
    private List<Dtos.HubDtos.UserDto> Users { get; set; } = [];

    public string? GetById(int id)
        => Users.Where(i => i.Id == id)
            .Select(c => c.ConnectionId)
            .SingleOrDefault();
    public void Add(int id, string connId)
        => Users.Add(
            new Dtos.HubDtos.UserDto { Id = id, ConnectionId = connId }
        );
    public void Remove(string connId)
        => Users.RemoveAll(c => c.ConnectionId == connId);
}
