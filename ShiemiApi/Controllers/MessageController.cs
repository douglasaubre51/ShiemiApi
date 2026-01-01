namespace ShiemiApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class MessageController(
    MessageRepository messageRepo,
    ChannelRepository channelRepo,
    UserRepository userRepo,
    IHubContext<RoomHub> messageHub
)
{
    private readonly ChannelRepository _channelRepo = channelRepo;
    private readonly IHubContext<RoomHub> _messageHub = messageHub;
    private readonly MessageRepository _messageRepo = messageRepo;
    private readonly UserRepository _userRepo = userRepo;

    [HttpPost]
    public IResult CreateMessage(MessageDto dto)
    {
        try
        {
            var channel = _channelRepo.GetById(dto.ChannelId);
            if (channel is null)
                return Results.BadRequest("channel doesnot exists!");

            var user = _userRepo.GetById(dto.UserId);
            if (user is null)
                return Results.BadRequest("user doesnot exists!");

            Message message = new()
            {
                Text = dto.Text,
                Video = dto.Video,
                Photo = dto.Photo,
                Voice = dto.Voice,
                CreatedAt = dto.CreatedAt,
                Channel = channel,
                User = user
            };

            _messageRepo.Create(message);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("CreateMessage error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    [HttpGet("{Id}")]
    public IResult GetMessage(int Id)
    {
        try
        {
            var dbMessage = _messageRepo.GetById(Id);
            if (dbMessage is null)
                return Results.NotFound();

            return Results.Ok(dbMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetMessage error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    [HttpPut("{Id}")]
    public IResult UpdateMessage(int Id, Message message)
    {
        try
        {
            _messageRepo.Update(message);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("UpdateMessage error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    [HttpDelete("{Id}")]
    public IResult RemoveMessage(int Id)
    {
        try
        {
            _messageRepo.Remove(Id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("RemoveMessage error: " + ex.Message);
            return Results.BadRequest();
        }
    }
    [HttpDelete("all")]
    public IResult RemoveAll()
    {
        try
        {
            _messageRepo.RemoveAll();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }
}
