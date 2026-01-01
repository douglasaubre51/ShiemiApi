using System.Runtime.CompilerServices;
using AutoMapper;
using ShiemiApi.Storage.HubStorage;

namespace ShiemiApi.Hubs;

public class ProjectHub(
    ProjectStorage projectStorage,
    ProjectRepository projectRepository,
    IMapper mapper
) : Hub
{
    private readonly ProjectStorage _projectStorage = projectStorage;
    private readonly ProjectRepository _projectRepository = projectRepository;
    private readonly IMapper _mapper = mapper;

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("client connected on project: " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("client disconnected on project: " + Context.ConnectionId);
        _projectStorage.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Init(int userId)
    {
        try
        {
            _projectStorage.Add(userId, Context.ConnectionId);
            Console.WriteLine("user added to projectStorage: " + Context.ConnectionId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ProjectHub: Init error: {ex.Message}");
        }
    }

    public async Task Load(int userId)
    {
        try
        {
            List<Project> projects = _projectRepository.GetAllByUserId(userId);
            string? connId = _projectStorage.GetById(userId);
            if (connId is null)
                return;

            List<ProjectDto> dtos = _mapper.Map<List<ProjectDto>>(projects);
            await Clients.User(connId).SendAsync(
                "Load",
                dtos
                );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Load: error: {ex.Message}");
        }
    }
}
