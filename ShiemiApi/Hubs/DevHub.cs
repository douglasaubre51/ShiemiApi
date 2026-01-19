namespace ShiemiApi.Hubs;

public class DevHub(
    RoomRepository roomRepo,
    UserRepository userRepo,
    DevRepository devRepo,
    UserStorage userStorage
) : Hub
{
    private readonly RoomRepository _roomRepo = roomRepo;
    private readonly UserRepository _userRepo = userRepo;
    private readonly DevRepository _devRepo = devRepo;
    private readonly UserStorage _userStorage = userStorage;

    public override async Task OnConnectedAsync()
        => Console.WriteLine("client connected: " + Context.ConnectionId);
    public override Task OnDisconnectedAsync(Exception? ex)
    {
        _userStorage.Remove(Context.ConnectionId);
        Console.WriteLine("client disconnected: " + Context.ConnectionId);
        return base.OnDisconnectedAsync(ex);
    }

    // hub methods
    public async Task SetUserIdAndRoom(int userId, int devId, int roomId)
    {
        try
        {
            _userStorage.Add(userId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Caller.SendAsync(
                "LoadChat",
                _roomRepo.GetAllMessagesByRoomId(roomId, devId, RoomTypes.DEV)
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SetUserIdAndRoom error: {ex.Message}");
        }
    }
    public async Task SendChat(MessageDto dto)
    {
        var user = _userRepo.GetById(dto.UserId);
        var room = _roomRepo.GetById(dto.RoomId);
        if (user is null)
            return;
        if (room is null)
            return;

        Message message = new()
        {
            Text = dto.Text!,
            CreatedAt = dto.CreatedAt,
            User = user,
            Room = room
        };
        _roomRepo.AddMessage(dto.RoomId, message);
        Console.WriteLine($"{message.User.Id}: {message.Text}");

        await Clients.Groups(dto.RoomId.ToString())
            .SendAsync("UpdateChat", dto);
    }
}
