namespace ShiemiApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RoomController(
        RoomService roomService,
        RoomRepository roomRepository,
        ProjectRepository projectRepo,
        UserRepository userRepo,
        DevRepository devRepo
        )
{
    private readonly ProjectRepository _projectRepo = projectRepo;
    private readonly RoomService _roomService = roomService;
    private readonly RoomRepository _roomRepository = roomRepository;
    private readonly UserRepository _userRepo = userRepo;
    private readonly DevRepository _devRepo = devRepo;

    [HttpDelete("all")]
    public IResult ClearAll()
    {
        try
        {
            _roomRepository.RemoveAll();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

    [HttpPost("Private")]
    public IResult InitializePrivateRoom(GetPrivateRoomDto dto)
    {
        try
        {
            var user = _userRepo.GetById(dto.UserId);
            if (dto.RoomType == RoomTypes.PRIVATE)
            {
                // private chat room
                var project = _projectRepo.GetById(dto.ProjectId);
                var newRoom = _roomService.Initialize(
                        user,
                        project
                        );
                return newRoom is not null ?
                    Results.Ok(newRoom.Id) : Results.BadRequest("room is null!");
            }

            // dev room
            var dev = _devRepo.GetById(dto.DevId);
            var room = _roomService.Initialize(
                    user,
                    dev
                    );
            return room is not null ?
                Results.Ok(room.Id) : Results.BadRequest("room is null!");
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

	[HttpGet("Dev/{userId}/all")]
	public IResult GetAllDevRoomsByUserId(int userId)
	{
		try
		{
			var dbRooms = _roomRepository.GetQueryable()
				.Include(t => t.Tenant)
				.ThenInclude(p => p.ProfilePhoto)
				.Include(d => d.Dev)
				.Include(p => p.Project)
				.Where(u => u.Owner.Id == userId)
				.Where(r => r.RoomType == RoomTypes.DEV)
				.ToList();
			if(dbRooms.Count is 0)
			{
				return Results.BadRequest(new { Message = "empty list!" });
			}
			
			List<GetDevRoomDto> devRoomDtos = [];
			foreach(var r in dbRooms)
			{
				GetDevRoomDto dto = new ()
				{
					RoomId = r.Id,
					ClientId = r.Tenant.Id,
					ProfilePhotoURL = r.Tenant.ProfilePhoto.URL,
					ClientName = r.Tenant.FirstName + r.Tenant.LastName
				};
				devRoomDtos.Add(dto);
			}

			return Results.Ok(devRoomDtos);
		}
		catch(Exception ex)
		{
			Console.WriteLine(ex.Message);
			return Results.InternalServerError(new { Message = "error fetching dev rooms for current user!" });
		}
	}

    [HttpGet("Private/{userId}/all")]
    public IResult GetAllByUserId(int userId)
    {
        try
        {
            var rooms = _roomRepository.GetAllByUserId(userId);
            if (rooms.Count < 0)
                return Results.BadRequest();

            var roomListDto = new List<RoomDto>();
            foreach (var r in rooms)
            {
                var messages = new List<MessageDto>();
                foreach (var m in r.Messages!)
                {
                    var message = new MessageDto
                    {
                        Id = m.Id,
                        Text = m.Text,
                        Voice = m.Voice,
                        Video = m.Video,
                        Photo = m.Photo,
                        CreatedAt = m.CreatedAt,
                        UserId = m.User.Id,
                        RoomId = m.Room.Id
                    };
                    messages.Add(message);
                }
                var room = new RoomDto()
                {
                    Id = r.Id,
                    TenantId = r.Tenant.Id,
                    OwnerId = r.Owner.Id,
                    ProjectId = r.ProjectId ?? 0,
                    Messages = messages
                };
                roomListDto.Add(room);
            }

            return Results.Ok(new { Rooms = roomListDto });
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(ex.Message);
        }
    }

    [HttpGet("Private/{id}/messages")]
    public IResult GetAllMessagesById(int id)
    {
        try
        {
            var dtoCollection = _roomRepository.GetAllMessagesByRoomId(id);
            return dtoCollection is null ?
                Results.BadRequest("empty list!") : Results.Ok(dtoCollection);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(ex.Message);
        }
    }
}
