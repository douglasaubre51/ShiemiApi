namespace ShiemiApi.Repositories;

public class DevRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public Dev? GetById(int id)
        => _context.Devs.Include(u => u.User)
        .Include(p => p.Advert)
        .Include(d => d.DevRooms)
        .ThenInclude(devRooms => devRooms.Tenant)
        .SingleOrDefault(u => u.Id == id);

    public Dev? GetByUserId(int id)
        => _context.Devs.Include(u => u.User)
        .SingleOrDefault(u => u.UserId == id);

    public List<Dev> GetAll()
        => _context.Devs.Include(u => u.User)
        .ThenInclude(p => p!.ProfilePhoto)
        .ToList();

    public void Add(Dev dev)
    {
        _context.Devs.Add(dev);
        Save();
    }
    public void Update(Dev dev)
    {
        _context.Devs.Update(dev);
        Save();
    }
    public void Remove(int id)
    {
        var dev = _context.Devs.Single(u => u.Id == id);
        _context.Devs.Remove(dev);
        Save();
    }
    public void Save()
        => _context.SaveChanges();
    public IQueryable<Dev> GetQueryable()
        => _context.Devs;
}
