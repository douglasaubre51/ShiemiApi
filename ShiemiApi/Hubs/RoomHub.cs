namespace ShiemiApi.Hubs;

public class RoomHub(
        UserStorage userStorage,
        RoomRepository roomRepo,
        UserRepository userRepo
        ) : Hub
{
    private readonly RoomRepository _roomRepo = roomRepo;
    private readonly UserRepository _userRepo = userRepo;
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
    public async Task SetUserIdAndRoom(
            int id,
            int userId,
            int roomId,
            RoomTypes roomType)
    {
        try
        {
            _userStorage.Add(userId, Context.ConnectionId);

            Console.WriteLine("roomId: " + roomId);
            Console.WriteLine("project or devId: " + id);
            Console.WriteLine(roomType.ToString());
            var oldMessages = _roomRepo.GetAllMessagesByRoomId(roomId, id, roomType);
            Console.WriteLine($"no of messages: {oldMessages is null}");

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Caller.SendAsync(
                    "LoadChat",
                    _roomRepo.GetAllMessagesByRoomId(roomId, id, roomType)
                    );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Room: SetUserIdAndRoom error: {ex.Message}");
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

        await Clients.Groups(dto.RoomId.ToString())  // broadcast message !
            .SendAsync("UpdateChat", dto);
    }
}
