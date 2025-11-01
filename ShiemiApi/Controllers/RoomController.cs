using Microsoft.Identity.Client;

namespace ShiemiApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RoomController(
    RoomService roomService,
    RoomRepository roomRepository,
    ProjectRepository projectRepo,
    UserRepository userRepo
)
{
    private readonly ProjectRepository _projectRepo = projectRepo;
    private readonly RoomService _roomService = roomService;
    private readonly RoomRepository _roomRepository = roomRepository;
    private readonly UserRepository _userRepo = userRepo;

    [HttpGet("Private/{userId}/{projectId}")]
    public IResult InitializePrivateRoom(int userId, int projectId)
    {
        try
        {
            var user = _userRepo.GetById(userId);
            var project = _projectRepo.GetById(projectId);
            var room = _roomService.Initialize(
                user,
                project
            );
            return room is not null ?
                Results.Ok(room.Id) : Results.BadRequest("room is null!");
        }
        catch (Exception ex) { return Results.InternalServerError(ex); }
    }

    [HttpGet("Private/{userId}/all")]
    public IResult GetAllByUserId(int userId)
    {
        try
        {
           var rooms =  _roomRepository.GetAllByUserId(userId);
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
                   ProjectId = r.ProjectId,
                   Messages = messages
               };
               roomListDto.Add(room);
           }

           return Results.Ok(new {Rooms = roomListDto});
        }
        catch(Exception ex){ return Results.InternalServerError(ex.Message);}
    }
}