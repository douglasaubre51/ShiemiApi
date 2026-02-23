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
        Console.WriteLine("projectId: " + project.Id);

        var privateRooms = project.PrivateRooms;
        Console.WriteLine("is private rooms null: " + privateRooms.ToList());

        var exists = project.PrivateRooms.Where(r => r.Tenant.Id == tenant.Id)
            .Any(r => r.ProjectId == project.Id);

        if (exists is true)
            return project.PrivateRooms!.Where(r => r.Tenant.Id == tenant.Id)
                .Single(r => r.ProjectId == project.Id);

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
        return project.PrivateRooms!.Where(r => r.Tenant.Id == tenant.Id)
            .Single(r => r.ProjectId == project.Id);
    }

    // dev room init
    public Room Initialize(
        User tenant,
        Dev dev
    )
    {
        Console.WriteLine("searching for existing dev room....");

        var devRoomList = dev.DevRooms;

        var exists = dev.DevRooms.Where(r => r.Tenant!.Id == tenant.Id)
            .Any(r => r.Dev!.Id == dev.Id);
        Console.WriteLine($"dev room exists: {exists}");

        if (exists is true)
            return dev.DevRooms!.Where(r => r.Tenant!.Id == tenant.Id)
                .Single(r => r.Dev!.Id == dev.Id);

        Console.WriteLine("creating new dev room....");

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

        Console.WriteLine("created new dev room!");

        return dev.DevRooms!.Where(r => r.Tenant!.Id == tenant.Id)
            .Single(r => r.Dev!.Id == dev.Id);
    }
}
