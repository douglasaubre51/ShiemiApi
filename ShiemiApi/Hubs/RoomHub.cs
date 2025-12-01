using ShiemiApi.Storage.HubStorage;

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
    public async Task SetUserIdAndRoom(int userId, int roomId)
    {
        try
        {
            _userStorage.Add(userId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            Console.WriteLine($"creating room: {roomId}");

            // send all chats
            await Clients.Caller.SendAsync(
                "LoadChat",
                _roomRepo.GetAllMessagesByRoomId(roomId)
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SetUserIdAndRoom error: {ex.Message}");
        }
    }

    public async Task SendChat(MessageDto dto)
    {
        var user = _userRepo.GetById(dto.UserId) ?? null!;
        var room = _roomRepo.GetById(dto.RoomId) ?? null!;
        if (user is null)
        {
            Console.WriteLine("SendChat: error: user is null");
            return;
        }
        if (room is null)
        {
            Console.WriteLine("SendChat: error: room is null");
            return;
        }

        Message message = new()
        {
            Text = dto.Text,
            CreatedAt = dto.CreatedAt,
            User = user,
            Room = room
        };
        _roomRepo.AddMessage(dto.RoomId, message);

        // broadcast new message
        await Clients.Groups(dto.RoomId.ToString())
            .SendAsync("UpdateChat", dto);
    }
}