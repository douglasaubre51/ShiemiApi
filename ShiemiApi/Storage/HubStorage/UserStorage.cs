using ShiemiApi.Dtos.HubDtos;

namespace ShiemiApi.Storage.HubStorage;

public class UserStorage
{
    private List<UserDto> Users { get; set; } = [];

    public List<string> GetById(int id)
        => Users.Where(i => i.Id == id)
            .Select(c => c.ConnectionId).ToList();
    
    public void Add(int id,string connId)
        => Users.Add(
            new UserDto { Id = id, ConnectionId = connId }
        );

    public void Remove(string connId)
        => Users.RemoveAll(c => c.ConnectionId == connId);
}