namespace ShiemiApi.Services;

public class UserStorageService
{
    private Dictionary<int, string> Connections { get; } = [];

    public Dictionary<int, string> GetConnectionDict() => Connections;
    public string GetConnectionId(int userId) => Connections[userId];
    public void SetConnectionId(int userId, string connectionId)
        => Connections.Add(userId, connectionId);
    public void RemoveConnectionId(int userId)
        => Connections.Remove(userId);
}