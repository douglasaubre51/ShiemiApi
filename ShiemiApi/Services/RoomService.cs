namespace ShiemiApi.Services;

public class RoomService(
    UserRepository userRepo,
    ProjectRepository projectRepo
)
{
    private readonly ProjectRepository _projectRepo = projectRepo;
    private readonly UserRepository _userRepo = userRepo;

    public Room Initialize(
        User tenant,
        Project project
    )
    {
        Console.WriteLine("inside RoomService");
        // check if room already exists!
        var exists = project.PrivateRooms.Any(r => r.Tenant.Id == tenant.Id);
        Console.WriteLine($"room exists: {exists}");
        if (exists is true)
            return project.PrivateRooms!.Single(r => r.Tenant.Id == tenant.Id);
        
        // else create new room!
        Room newRoom = new()
        {
            Owner = _userRepo.GetById(project.UserId),
            Tenant = tenant,
            Project = project,
            ProjectId = project.Id
        };
        _projectRepo.AddPrivateRoom(newRoom, project.Id);
        
        // fetch and send!
        return project.PrivateRooms!.Single(r => r.Tenant.Id == tenant.Id);
    }
}