using ShiemiApi.Storage.HubStorage;

namespace ShiemiApi.Hubs;

public class ChannelHub(
    UserStorage userStorage,
    ChannelRepository channelRepository
) : Hub
{
    private readonly UserStorage _userStorage = userStorage;
    private readonly ChannelRepository _channelRepo = channelRepository;

    public override async Task OnConnectedAsync()
        => Console.WriteLine($"channel user: {Context.ConnectionId}");
    public override Task OnDisconnectedAsync(Exception? ex)
    {
        Console.WriteLine($"connection lost: {Context.ConnectionId}");
        _userStorage.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(ex);
    }

    public async Task Init(int userId, int channelId)
    {
        try
        {
            _userStorage.Add(userId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, channelId.ToString());

            // get all messages
            var channel = _channelRepo.GetById(channelId);
            List<MessageDto> messages = [];
            if (channel!.Messages is null)
            {
                await Clients.Caller.SendAsync("LoadChat",messages);
                return;
            }
            // pack all messages
            foreach (var m in channel.Messages)
            {
                MessageDto dto = new ()
                {
                    Id = m.Id,
                    Text = m.Text,
                    Voice = m.Voice,
                    Video = m.Video,
                    Photo = m.Photo,
                    CreatedAt = m.CreatedAt,
                    UserId = m.User.Id,
                    ChannelId = m.Channel!.Id
                };
                messages.Add(dto);
            }
            await Clients.Caller.SendAsync("LoadChat", messages);
        }
        catch (Exception ex) { Console.WriteLine($"init channel error: {ex.Message}"); }
    }
}