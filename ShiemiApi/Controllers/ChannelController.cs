namespace ShiemiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController(
        ProjectRepository projectRepo,
        ChannelRepository channelRepo
    )
    {
        private readonly ProjectRepository _projectRepo = projectRepo;
        private readonly ChannelRepository _channelRepo = channelRepo;

        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            try
            {
                Channel? dbChannel = _channelRepo.GetById(id);
                if (dbChannel is null)
                    return Results.BadRequest(new { Message = "Channel doesnot exist !" });

                Mapper mapper = MapperUtility.Get<Channel, ChannelDto>();
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
}
