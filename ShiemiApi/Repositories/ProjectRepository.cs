namespace ShiemiApi.Repositories;

public class ProjectRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public void Create(Project project)
    {
        _context.Projects.Add(project);
        Save();
    }
    public void AddPrivateRoom(Room room, int id)
    {
        var project = _context.Projects
            .Include(p => p.PrivateRooms)
            .SingleOrDefault(p => p.Id == id);
        if (project!.PrivateRooms is null)
            return;

        project.PrivateRooms.Add(room);
        Save();
    }

    public Project? GetById(int id)
        => _context.Projects.Include(p => p.PrivateRooms)
            .ThenInclude(privateRooms => privateRooms.Tenant)
            .Include(c => c.Channel)
            .SingleOrDefault(p => p.Id == id);
    public List<Project> GetAll()
        => _context.Projects.Include(c => c.Channel)
                            .ToList();
    public List<Project> GetAllByUserId(int UserId)
        => _context.Projects.Include(u => u.User)
        .Include(c => c.Channel)
        .Where(e => e.UserId == UserId)
        .ToList();

    public void Update(Project project)
    {
        _context.Projects.Update(project);
        Save();
    }
    public void Remove(int Id)
    {
        var project = _context.Projects.Single(p => p.Id == Id);
        _context.Projects.Remove(project);
        Save();
    }
    public void Save()
        => _context.SaveChanges();
    public IQueryable<Project> GetQueryable()
        => _context.Projects;
}
