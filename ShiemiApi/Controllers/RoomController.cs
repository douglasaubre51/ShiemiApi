namespace ShiemiApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class RoomController(
    RoomService roomService,
    ProjectRepository projectRepo,
    UserRepository userRepo
)
{
    private readonly ProjectRepository _projectRepo = projectRepo;
    private readonly RoomService _roomService = roomService;
    private readonly UserRepository _userRepo = userRepo;

    [HttpGet("/private-room/init/{userId}/{projectId}")]
    public IResult InitializePrivateRoom(int userId, int projectId)
    {
        try
        {
            var room = _roomService.Initialize(
                _userRepo.GetById(userId),
                _projectRepo.GetById(projectId)
            );

            return Results.Ok(room);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"InitPrivateRoom error: {ex.Message}");
            return Results.InternalServerError();
        }
    }
}