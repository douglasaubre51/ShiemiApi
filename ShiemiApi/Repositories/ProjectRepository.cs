namespace ShiemiApi.Repositories
{

    public class ProjectRepository(ApplicationDbContext context)
    {

        private readonly ApplicationDbContext _context = context;

        public void Save()
            => _context.SaveChanges();

        // Create

        public void Create(Project project)
        {
            _context.Projects
            .Add(project);

            Save();
        }

	public void AddPrivateRoom(Room room, int id)
	{
	    Project project = _context.Projects
		.Where( p => p.Id == id )
		.Single();

	    project.PrivateRooms.Add(room);

	    Save();
	}

        // Read

        public Project GetById(int id)
            => _context.Projects
            .Where(u => u.Id == id)
            .SingleOrDefault();

        public List<Project> GetAll()
            => _context.Projects
            .ToList();

        public List<Project> GetAllByUserId(int UserId)
            => _context.Projects
            .Where(e => e.UserId == UserId)
            .ToList();

        // Update

        public void Update(Project project)
        {
            _context.Projects
            .Update(project);

            Save();
        }

        // Delete

        public void Remove(int Id)
        {
            var project = _context.Projects
            .Where(p => p.Id == Id)
            .Single();

            _context.Projects
            .Remove(project);

            Save();
        }
    }
}

