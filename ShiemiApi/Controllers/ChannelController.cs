namespace ShiemiApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChannelController(
    ProjectRepository projectRepo,
    ChannelRepository channelRepo,
    UserRepository userRepo
)
{
    private readonly UserRepository _userRepo = userRepo;
    private readonly ProjectRepository _projectRepo = projectRepo;
    private readonly ChannelRepository _channelRepo = channelRepo;

    [HttpGet("{projectId}/user/all")]
    public IResult GetAllChannelUsers(int projectId)
    {
        try
        {
            Project dbProject = _projectRepo.GetById(projectId);
            if (dbProject is null)
                return Results.BadRequest(new { Message = "Channel doesnt exist!" });

            List<GetAllChannelUserDto> channelDtos = [];

            foreach(var userId in dbProject.UserList)
            {
                User dbUser = _userRepo.GetById(userId);

                GetAllChannelUserDto dto = new GetAllChannelUserDto(
                    UserId: dbUser.Id,
                    Username: dbUser.FirstName + " " + dbUser.LastName,
                    Profile: dbUser.ProfilePhoto.URL
                        );

                channelDtos.Add(dto);
            }


            return Results.Ok(channelDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Channel: getAll: error: " + ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpGet("all")]
    public IResult GetAll()
    {
        try
        {
            List<Channel> dbChannels = _channelRepo.GetAll();
            if (dbChannels is null || dbChannels.Count is 0)
                return Results.BadRequest(new { Message = "Empty list!" });

            List<GetAllChannelDto> channelDtos = [];

            foreach(var channel in dbChannels)
            {
                GetAllChannelDto dto = new GetAllChannelDto(
                    Id: channel.Id,
                    ProjectId: channel.Project.Id,
                    UserId: channel.Project.UserId,
                    Username: channel.Project.User.FirstName + " " + channel.Project.User.LastName,
                    UserProfile: channel.Project.User.ProfilePhoto.URL,
                    Title: channel.Project.Title
                        );

                channelDtos.Add(dto);
            }


            return Results.Ok(channelDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Channel: getAll: error: " + ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    public IResult Get(int id)
    {
        try
        {
            Channel? dbChannel = _channelRepo.GetById(id);
            if (dbChannel is null)
                return Results.BadRequest(new { Message = "Channel doesnot exist !" });

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Channel, ChannelDto>()
                    .ForMember(m => m.Messages, option => option.Ignore()),
                new LoggerFactory()
            );
            Mapper mapper = new(config);
            ChannelDto dto = mapper.Map<ChannelDto>(dbChannel);

            mapper = MapperUtility.Get<Project, ProjectDto>();
            dto.CurrentProject = mapper.Map<ProjectDto>(dbChannel.Project);

            if (dbChannel.Messages!.Count > 0)
                foreach (var c in dbChannel.Messages!)
                    dto.Messages.Add(c.Id);

            return Results.Ok(new { Channel = dto });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Channel: get: error: " + ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpPost("add-channel")]
    public IResult AddToProject(AddChannelDto dto)
    {
        try
        {
            Console.WriteLine($"projectId: {dto.ProjectId}");
            Console.WriteLine($"userId: {dto.UserId}");

            Project? dbProject = _projectRepo.GetById(dto.ProjectId);
            if (dbProject is null)
                return Results.BadRequest(new { Message = "Project not found !" });

            dbProject.UserList.Add(dto.UserId);
            _channelRepo.Save();

            if (dbProject.Channel is not null)
                return Results.Ok();

            Console.WriteLine("creating new channel ...");
            dbProject.Channel = new()
            {
                ProjectId = dto.ProjectId,
                Title = dbProject.Title,
                Project = dbProject,
            };
            _channelRepo.Save();

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("AddToProject error: " + ex.Message);
            return Results.InternalServerError();
        }
    }
}
