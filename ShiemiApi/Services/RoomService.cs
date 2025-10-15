namespace ShiemiApi.Services;


public class RoomService(
	UserRepository userRepo,
	ProjectRepository projectRepo
	)
{
    private readonly UserRepository _userRepo = userRepo;
    private readonly ProjectRepository _projectRepo = projectRepo;


    public Room Initialize(
	    User tenant,
	    Project project
	    )
    {
	bool exists = project.PrivateRooms.Any( r => r.Tenant.Id == tenant.Id );
	if(exists is false)
	{
	    Room newRoom = new ()
	    {
		Owner = _userRepo.GetById(project.UserId),
		      Tenant = tenant,
		      Project = project,
		      ProjectId = project.Id
	    };

	    _projectRepo.AddPrivateRoom(newRoom, project.Id);

	    return project.PrivateRooms
		.Where( r => r.Tenant.Id == tenant.Id )
		.SingleOrDefault();
	}

	return project.PrivateRooms
	    .Where( r => r.Tenant.Id == tenant.Id )
	    .SingleOrDefault();
    }
}
