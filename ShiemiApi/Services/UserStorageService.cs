namespace ShiemiApi.Services
{
    public class UserStorageService
    {
        private Dictionary<string, string> Connections { get; set; } = [];

        public string GetConnectionId(string userId)
            => Connections[userId];

        public void SetConnectionId(string userId, string connectionId)
            => Connections.Add(userId, connectionId);

        public void RemoveConnectionId(string userId)
            => Connections.Remove(userId);
    }
}
