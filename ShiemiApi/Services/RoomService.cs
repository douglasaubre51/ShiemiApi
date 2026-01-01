namespace ShiemiApi.Services;

public class RoomService(
    UserRepository userRepo,
    ProjectRepository projectRepo,
    DevRepository devRepo,
    RoomRepository roomRepo
)
{
    private readonly ProjectRepository _projectRepo = projectRepo;
    private readonly UserRepository _userRepo = userRepo;
    private readonly DevRepository _devRepo = devRepo;
    private readonly RoomRepository _roomRepo = roomRepo;

    // private room init
    public Room Initialize(
        User tenant,
        Project project
    )
    {
        // check if room already exists!
        var exists = project.PrivateRooms.Any(r => r.Tenant.Id == tenant.Id);
        if (exists is true)
            return project.PrivateRooms!.Single(r => r.Tenant.Id == tenant.Id);

        // else create new room!
        Room newRoom = new()
        {
            Owner = _userRepo.GetById(project.UserId)!,
            Tenant = tenant,
            Project = project,
            ProjectId = project.Id,
            RoomType = RoomTypes.PRIVATE
        };
        _roomRepo.Add(newRoom);

        // fetch and send!
        return project.PrivateRooms!.Single(r => r.Tenant.Id == tenant.Id);
    }

    // dev room init
    public Room Initialize(
        User tenant,
        Dev dev
    )
    {
        var exists = dev.DevRooms.Any(r => r.Tenant.Id == tenant.Id);
        if (exists is true)
            return dev.DevRooms!.Single(r => r.Tenant.Id == tenant.Id);

        Room newRoom = new()
        {
            Owner = _userRepo.GetById(dev.UserId)!,
            Tenant = tenant,
            ProjectId = null,
            Project = null,
            Dev = dev,
            DevId = dev.Id,
            RoomType = RoomTypes.DEV
        };
        _roomRepo.Add(newRoom);

        return dev.DevRooms!.Single(r => r.Tenant.Id == tenant.Id);
    }
}
