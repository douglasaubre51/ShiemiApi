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
        var exists = project.PrivateRooms!.Any(r => r.Tenant.Id == tenant.Id);
        if (exists is true)
            return project.PrivateRooms!.Single(r => r.Tenant.Id == tenant.Id);
        {
            Room newRoom = new()
            {
                Owner = _userRepo.GetById(project.UserId),
                Tenant = tenant,
                Project = project,
                ProjectId = project.Id
            };

            _projectRepo.AddPrivateRoom(newRoom, project.Id);
            return project.PrivateRooms!.Single(r => r.Tenant.Id == tenant.Id);
        }
    }
}