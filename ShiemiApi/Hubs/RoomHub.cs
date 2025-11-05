using System.Diagnostics;
using System.Text.RegularExpressions;
using ShiemiApi.Storage.HubStorage;

namespace ShiemiApi.Hubs;

public class RoomHub(
    UserStorage userStorage,
    RoomRepository roomRepo,
    UserRepository userRepo
) : Hub
{
    private readonly UserStorage _userStorage = userStorage;
    private readonly RoomRepository _roomRepo = roomRepo;
    private readonly UserRepository _userRepo = userRepo;

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("client connected: " + Context.ConnectionId);
    }
    public override Task OnDisconnectedAsync(Exception? ex)
    {
        Console.WriteLine("client disconnected: " + Context.ConnectionId);
        _userStorage.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(ex);
    }
    // hub methods
    public async Task SetUserIdAndRoom(int userId, int roomId)
    {
        _userStorage.Add(userId, Context.ConnectionId);
        await Groups.AddToGroupAsync( Context.ConnectionId, roomId.ToString() );
        await Clients.Caller .SendAsync( 
            "LoadChat",
            _roomRepo.GetAllMessagesByRoomId(roomId)
        );
    }
    public async Task SendChat(MessageDto dto)
    {
        Message message = new()
        {
            Text = dto.Text,
            CreatedAt = dto.CreatedAt,
            User = _userRepo.GetById(dto.UserId),
            Room = _roomRepo.GetById(dto.RoomId)
        };
        _roomRepo.AddMessage(dto.RoomId,message);
            
        await Clients.Groups(dto.RoomId.ToString())
            .SendAsync( "UpdateChat", dto );
    }
}