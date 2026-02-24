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
                Console.WriteLine("project id: " + project.Id);

                var newRoom = _roomService.Initialize(
                        user,
                        project
                        );

                return newRoom is not null ?
                    Results.Ok(newRoom.Id) : Results.BadRequest("room is null!");
            }

            Console.WriteLine($"userId: {dto.UserId}");
            Console.WriteLine($"ProjectId: {dto.ProjectId}");
            Console.WriteLine($"DevId: {dto.DevId}");
            Console.WriteLine($"RoomType: {dto.RoomType}");

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
            Console.WriteLine($"init private room error: " + ex.Message);
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

    [HttpGet("all/with-user")]
    public IResult GetAllWithUsers()
    {
        try
        {
            var dbRooms = _roomRepository.GetAll();
            if (dbRooms.Count is 0)
                return Results.BadRequest(new { Message = "empty list!" });

            List<GetAllRoomsWithUsersDto> dtos = [];
            foreach (var room in dbRooms)
            {
                GetAllRoomsWithUsersDto dto = new(
                        room.Id,
                        room.Owner.FirstName + " " + room.Owner.LastName,
                        room.Owner.ProfilePhoto?.URL,
                        room.Owner.Id,
                        room.Tenant.FirstName + " " + room.Tenant.LastName,
                        room.Tenant.ProfilePhoto?.URL,
                        room.Tenant.Id,
                        room.RoomType);

                dtos.Add(dto);
            }

            return Results.Ok(dtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public IResult GetById(int id)
    {
        try
        {
            var dbRoom = _roomRepository.GetById(id);
            if (dbRoom is null)
                return Results.BadRequest(new { Message = "room doesnt exist!" });

            return Results.Ok(new
            {
                Id = dbRoom.Id,
                OwnerName = dbRoom.Owner!.FirstName + " " + dbRoom.Owner.LastName,
                TenantName = dbRoom.Tenant!.FirstName + " " + dbRoom.Tenant.LastName,
                RoomType = dbRoom.RoomType
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "server error!" });
        }
    }

    [HttpGet("all")]
    public IResult GetAllRooms()
    {
        try
        {
            var dbRooms = _roomRepository.GetAll();
            if (dbRooms.Count is 0)
                return Results.BadRequest(new { Message = "empty list!" });

            List<GetAllRoomsDto> dtoList = [];
            foreach (var r in dbRooms)
            {
                GetAllRoomsDto dto = new(
                    RoomId: r.Id,
                    OwnerId: r.Owner.Id,
                    TenantId: r.Tenant.Id,
                    ProjectId: r.ProjectId,
                    DevId: r.DevId,
                    RoomType: r.RoomType
                );
                dtoList.Add(dto);
            }

            return Results.Ok(dtoList);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "server error!" });
        }
    }


    [HttpGet("Dev/all")]
    public IResult GetAll()
    {
        try
        {
            var dbRooms = _roomRepository.GetAll();
            if (dbRooms.Count is 0)
                return Results.BadRequest(new { Message = "empty list!" });

            List<GetAllRoomsDto> dtoList = [];
            foreach (var r in dbRooms)
            {
                GetAllRoomsDto dto = new(
                    RoomId: r.Id,
                    OwnerId: r.Owner.Id,
                    TenantId: r.Tenant.Id,
                    ProjectId: r.ProjectId,
                    DevId: r.DevId,
                    RoomType: r.RoomType
                );
                dtoList.Add(dto);
            }

            return Results.Ok(dtoList);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "server error!" });
        }
    }

    [HttpGet("Client/{userId}/all")]
    public IResult GetAllDevProfilesForClient(int userId)
    {
        try
        {
            var dbRooms = _roomRepository.GetQueryable()
                .Include(t => t.Owner)
                .ThenInclude(p => p!.ProfilePhoto)
                .Include(d => d.Dev)
                .Where(u => u.Tenant!.Id == userId)
                .Where(r => r.RoomType == RoomTypes.DEV)
                .ToList();
            if (dbRooms.Count is 0)
            {
                Console.WriteLine("empty list!");
                return Results.BadRequest(new { Message = "empty list!" });
            }

            List<GetDevProfileDto> devProfileDtos = [];

            foreach (var r in dbRooms)
            {
                GetDevProfileDto dto = new(
                    RoomId: r.Id,
                    OwnerId: r.Owner!.Id,
                    OwnerProfile: r.Owner.ProfilePhoto!.URL,
                    OwnerName: r.Owner.FirstName + " " + r.Owner.LastName,
                    DevId: r.DevId ?? 0
                );

                devProfileDtos.Add(dto);
            }

            return Results.Ok(devProfileDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "error fetching dev rooms for current user!" });
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
            if (dbRooms.Count is 0)
            {
                Console.WriteLine("empty list!");
                return Results.BadRequest(new { Message = "empty list!" });
            }

            List<GetDevRoomDto> devRoomDtos = [];
            foreach (var r in dbRooms)
            {
                GetDevRoomDto dto = new()
                {
                    RoomId = r.Id,
                    ClientId = r.Tenant.Id,
                    ProfilePhotoURL = r.Tenant.ProfilePhoto?.URL,
                    ClientName = r.Tenant.FirstName + " " + r.Tenant.LastName
                };
                devRoomDtos.Add(dto);
            }

            return Results.Ok(devRoomDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "error fetching dev rooms for current user!" });
        }
    }

    [HttpGet("Private/{projectId}/{userId}/all")]
    public IResult GetAllByUserId(int projectId, int userId)
    {
        try
        {
            var rooms = _roomRepository.GetAllByUserId(userId, projectId);
            if (rooms.Count < 0)
                return Results.BadRequest();

            var roomListDto = new List<RoomDto>();
            foreach (var r in rooms)
            {
                if (r.RoomType == RoomTypes.DEV)
                    continue;

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
                if (r.RoomType == RoomTypes.DEV)
                    continue;

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

    [HttpGet("Private/{id}/{projectId}/messages")]
    public IResult GetAllMessagesById(int id, int projectId)
    {
        try
        {
            var dtoCollection = _roomRepository.GetAllMessagesByRoomId(id, projectId, RoomTypes.PRIVATE);
            return dtoCollection is null ?
                Results.BadRequest("empty list!") : Results.Ok(dtoCollection);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(ex.Message);
        }
    }
}
