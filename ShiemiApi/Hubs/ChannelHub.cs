namespace ShiemiApi.Hubs;

public class ChannelHub(
    UserStorage userStorage,
    ChannelRepository channelRepository,
    UserRepository userRepository
) : Hub
{
    private readonly UserStorage _userStorage = userStorage;
    private readonly UserRepository _userRepo = userRepository;
    private readonly ChannelRepository _channelRepo = channelRepository;

    public override async Task OnConnectedAsync()
        => Console.WriteLine($"channel user: {Context.ConnectionId}");
    public override Task OnDisconnectedAsync(Exception? ex)
    {
        Console.WriteLine($"connection lost: {Context.ConnectionId}");
        _userStorage.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(ex);
    }

    public async Task UploadChat(MessageDto dto)
    {
        try
        {
            var user = _userRepo.GetById(dto.UserId);
            var channel = _channelRepo.GetById(dto.ChannelId);
            if (user is null)
            {
                Console.WriteLine("SendChat: error: user is null");
                return;
            }
            if (channel is null)
            {
                Console.WriteLine("SendChat: error: room is null");
                return;
            }

            Message message = new()
            {
                Text = dto.Text!,
                CreatedAt = dto.CreatedAt,
                User = user,
                Channel = channel
            };
            _channelRepo.AddMessage(dto.ChannelId, message);
            Console.WriteLine($"{message.User.Id}: {message.Text}");

            // Add username to message !
            dto.Username = user.FirstName + " " + user.LastName;

            await Clients.Groups(channel.Id.ToString())  // broadcast message !
                .SendAsync("UpdateChat", dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Upload Chat: error: {ex.Message}");
        }
    }

    public async Task Init(int userId, int channelId)
    {
        try
        {
            var channel = _channelRepo.GetById(channelId);
            List<MessageDto> messages = [];
            if (channel is null)
                return;

            _userStorage.Add(userId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, channel.Id.ToString());

            foreach (var m in channel.Messages!)
            {
                MessageDto dto = new()
                {
                    Id = m.Id,
                    Text = m.Text,
                    Voice = m.Voice,
                    Video = m.Video,
                    Photo = m.Photo,
                    CreatedAt = m.CreatedAt,
                    UserId = m.User!.Id,
                    ChannelId = m.Channel!.Id,
                    Username = m.User.FirstName + " " + m.User.LastName
                };
                messages.Add(dto);
            }

            await Clients.Caller.SendAsync("LoadChat", messages);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"init channel error: {ex.Message}");
        }
    }
}