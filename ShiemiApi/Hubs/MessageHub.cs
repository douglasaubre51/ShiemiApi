namespace ShiemiApi.Hubs
{
    public class MessageHub(UserStorageService userStorage) : Hub
    {
        private readonly UserStorageService _userStorage = userStorage;

        public override async Task OnConnectedAsync()
            => Console.WriteLine("client connected: " + Context.ConnectionId);

        public void SetUserId(string userId)
            => userStorage.SetConnectionId(userId, Context.ConnectionId);
    }
}
