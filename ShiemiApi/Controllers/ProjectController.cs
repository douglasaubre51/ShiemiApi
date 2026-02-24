namespace ShiemiApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProjectController(
        ProjectRepository projectRepo,
        RoomRepository roomRepo,
        ChannelRepository channelRepo,
        UserRepository userRepo
        )
{
    private readonly UserRepository _userRepo = userRepo;
    private readonly ChannelRepository _channelRepo = channelRepo;
    private readonly ProjectRepository _projectRepo = projectRepo;
    private readonly RoomRepository _roomRepo = roomRepo;

    [HttpGet("{projectId}/delete")]
    public IResult DeleteProject(int projectId)
    {
        try
        {
            _projectRepo.Remove(projectId);

            return Results.Ok();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"DeleteProject post: error: {ex.Message}");

            return Results.InternalServerError();
        }
    }

    [HttpPost("edit")]
    public IResult EditProject(EditProjectDto dto)
    {
        try
        {
            var dbProject = _projectRepo.GetById(dto.Id);
            dbProject.Title = dto.Title;
            dbProject.ShortDesc = dto.ShortDesc;
            dbProject.Description = dto.Description;

            _projectRepo.Save();

            return Results.Ok();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"EditProject post: error: {ex.Message}");
            return Results.InternalServerError();
        }
    }

    [HttpGet("{projectId}/init/invite-list")]
    public IResult InitInviteList(int projectId)
    {
        try
        {
            var inviteRequests = _projectRepo.GetById(projectId)
                .InviteList;
            inviteRequests = new List<int>();
            _projectRepo.Save();

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpGet("{projectId}/joined-users-count")]
    public IResult GetJoinedUsersCountById(int projectId)
    {
        try
        {
            var userCount = _projectRepo.GetById(projectId)
                .UserList
                .ToList()
                .Count();

            return Results.Ok(userCount);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = ex.Message });
        }
    }

    [HttpGet("{title}/search")]
    public IResult GetSearchedProjects(string title)
    {
        try
        {
            var projects = _projectRepo.SearchByTitle(title);
            if (projects.Count is 0)
                return Results.BadRequest(new { Message = "empty list!" });

            return Results.Ok(projects.Select(project => new
            {
                Id = project.Id,
                ShortDesc = project.ShortDesc,
                Title = project.Title,
                UserId = project.UserId
            }));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = $"error: {ex.Message}" });
        }
    }

    [HttpGet("{projectId}/invite-request/all")]
    public IResult GetAllInviteRequests(int projectId)
    {
        try
        {
            var inviteRequests = _projectRepo.GetById(projectId)
                .InviteList
                .ToList();
            if (inviteRequests.Count is 0)
                return Results.BadRequest(new { Message = "empty list" });

            return Results.Ok(inviteRequests);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpGet("{projectId}/{userId}/send-request-admin")]
    public IResult SendAdminInviteRequest(int projectId, int userId)
    {
        try
        {
            _projectRepo.GetById(projectId)
                .InviteList
                .Add(userId);
            return Results.Ok(new { Message = $"{userId} sent an invite to project admin!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpGet("{projectId}/joined-client/all")]
    public IResult GetAllJoinedClientId(int projectId)
    {
        try
        {
            var dbProject = _projectRepo.GetById(projectId);
            if (dbProject is null)
                return Results.BadRequest(new { Message = "project doesnot exists!" });

            var clientList = dbProject.UserList.ToList();
            return Results.Ok(clientList);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpGet("{projectId}/{clientId}/add-client")]
    public IResult AddClient(int projectId, int clientId)
    {
        try
        {
            var dbProject = _projectRepo.GetById(projectId);
            if (dbProject is null)
                return Results.BadRequest(new { Message = "project doesnot exists!" });

            // Add user to project !
            dbProject.UserList.Add(clientId);
            _projectRepo.Save();

            // Add project to user participated projects !
            _userRepo.GetById(clientId)!
                .PastProjects!
                .Add(dbProject.Id);
            _userRepo.Save();

            return Results.Ok(new { Message = "client added to project!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpGet("{projectId}/{clientId}/remove-client")]
    public IResult RemoveClient(int projectId, int clientId)
    {
        try
        {
            var dbProject = _projectRepo.GetById(projectId);
            if (dbProject is null)
                return Results.BadRequest(new { Message = "project doesnot exists!" });

            dbProject.UserList.Remove(clientId);
            _projectRepo.Save();
            return Results.Ok(new { Message = "client removed from project!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError();
        }
    }

    [HttpPost]
    public async Task<IResult> CreateProject(ProjectDto dto)
    {
        try
        {
            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                ShortDesc = dto.ShortDesc,
                UserId = dto.UserId
            };
            _projectRepo.Create(project);

            Project? dbProject = await _projectRepo.GetQueryable()
                .SingleOrDefaultAsync(project => project.UserId == dto.UserId);
            if (dbProject is null)
                return Results.BadRequest(new { Message = "Couldnt create new Project!" });

            // Add new Channel for the Project !
            Channel channel = new Channel
            {
                ProjectId = dbProject.Id
            };
            _channelRepo.Add(channel);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("CreateProject error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    [HttpGet("{projectId}/devs-contacted/all")]
    public async Task<IResult> GetAllProjectCandidates(int projectId)
    {
        try
        {
            List<User?> dbUsers = await _roomRepo.GetQueryable()
                .Include(u => u.Tenant)
                .Where(p => p.ProjectId == projectId)
                .Select(u => u.Tenant)
                .ToListAsync();
            if (dbUsers.Count is 0)
                return Results.BadRequest(new { Message = "empty list!" });

            return Results.Ok(dbUsers);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "error fetching user details!" });
        }
    }

    [HttpGet("all/{userId}/user-joined")]
    public IResult GetUserJoinedProjects(int userId)
    {
        try
        {
            var dbProjects = _projectRepo.GetAll()
                .Where(p => p.UserList.Contains(userId))
                .ToList();
            if (dbProjects.Count is 0)
                return Results.BadRequest(new { Message = "empty list!" });

            Mapper mapper = MapperUtility.Get<Project, ProjectDto>();
            List<ProjectDto> projectDtos = mapper.Map<List<ProjectDto>>(dbProjects);
            return Results.Ok(new { Projects = projectDtos });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.InternalServerError(new { Message = "error fetching projects!" });
        }
    }

    [HttpGet("all")]
    public IResult GetAll()
    {
        try
        {
            var dbProjects = _projectRepo.GetAll();
            if (dbProjects is null)
                return Results.NotFound();

            Mapper mapper = MapperUtility.Get<Project, ProjectDto>();
            List<ProjectDto> dtos = mapper.Map<List<ProjectDto>>(dbProjects);

            return Results.Ok(new { Projects = dtos });
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetAllProjects error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    [HttpGet("all/{UserId}")]
    public IResult GetAllByUserId(int UserId)
    {
        try
        {
            Console.WriteLine($"user id: {UserId}");
            var projects = _projectRepo.GetAllByUserId(UserId);
            if (projects is null)
                return Results.BadRequest(new { Message = "Empty projects !" });

            List<Project> allProjects = _projectRepo.GetAll()
                .Where(u => u.UserList.Contains(UserId))
                .ToList();
            allProjects.ForEach(p => Console.WriteLine($"Project Title: {p.Title}"));
            projects.AddRange(allProjects);

            var map = MapperUtility.Get<Project, ProjectDto>();
            List<ProjectDto> dtos = map.Map<List<ProjectDto>>(projects);

            for (int i = 0; i < projects.Count; i++)
            {
                if (projects[i].Channel is null)
                    continue;

                dtos[i].ChannelId = projects[i].Channel!.Id;
            }

            return Results.Ok(new { Projects = dtos });
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetAllProjectByUserId error: " + ex.Message);
            return Results.InternalServerError(ex.Message);
        }
    }

    [HttpGet("{ProjectId}")]
    public IResult GetById(int ProjectId)
    {
        try
        {
            var dbProject = _projectRepo.GetById(ProjectId);
            if (dbProject is null)
                return Results.NotFound();

            if (dbProject.Channel is null)
                _channelRepo.Add(new Channel
                {
                    ProjectId = dbProject.Id
                });

            Mapper mapper = MapperUtility.Get<Project, ProjectDto>();
            ProjectDto dto = mapper.Map<ProjectDto>(dbProject);

            return Results.Ok(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetProjectById error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    [HttpPut("{Id}")]
    public IResult UpdateProject(int Id, Project project)
    {
        try
        {
            _projectRepo.Update(project);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("UpdateProject error: " + ex.Message);
            return Results.BadRequest();
        }
    }

    [HttpDelete("{Id}")]
    public IResult RemoveProject(int Id)
    {
        try
        {
            _projectRepo.Remove(Id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine("RemoveProject error: " + ex.Message);
            return Results.BadRequest();
        }
    }
}
