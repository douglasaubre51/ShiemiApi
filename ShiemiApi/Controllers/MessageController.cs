namespace ShiemiApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MessageController(
		MessageRepository messageRepo,
		ChannelRepository channelRepo,
		IHubContext<MessageHub> messageHub
	)
    {
		private readonly MessageRepository _messageRepo = messageRepo;
        private readonly ChannelRepository _channelRepo = channelRepo;
        private readonly IHubContext<MessageHub> _messageHub = messageHub; 

        [HttpPost("/create")]
        public IResult CreateMessage(MessageDto dto)
        {
            try
            {
				var channel = _channelRepo.GetById(dto.ChannelId);
				if(channel is null)
					return BadRequest("channel doesnot exists!");

				var user = _userRepo.GetById(dto.UserId);
				if(user is null)
					return BadRequest("user doesnot exists!");
				
				Message message = new ()
				{
					Text = dto.Text,
					Video = dto.Video,
					Photo = dto.Photo,
					Voice = dto.Voice,
					CreatedAt = CreatedAt,
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

        [HttpGet("/get-message/{Id}")]
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

        [HttpPut("/update-message/{Id}")]
        public IResult UpdateMessage(int Id,Message message)
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

        [HttpDelete("/remove-message/{Id}")]
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
    }
}
