namespace ShiemiApi.Repositories;

public class ProjectRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public void Save()
    {
        _context.SaveChanges();
    }

    // Create

    public void Create(Project project)
    {
        _context.Projects
            .Add(project);

        Save();
    }

    public void AddPrivateRoom(Room room, int id)
    {
        var project = _context.Projects
            .Include(p=>p.PrivateRooms)
            .Where(p => p.Id == id)
            .Single();

        Console.WriteLine($"project selected!: {project.Title}");

        project.PrivateRooms.Add(room);

        Save();
    }

    // Read

    public Project GetById(int id)
    {
        return _context.Projects.Include(p => p.PrivateRooms)
            .ThenInclude(t=>t.Tenant)
            .Single(u => u.Id == id);
    }

    public List<Project> GetAll()
    {
        return _context.Projects
            .ToList();
    }

    public List<Project> GetAllByUserId(int UserId)
    {
        return _context.Projects
            .Where(e => e.UserId == UserId)
            .ToList();
    }

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