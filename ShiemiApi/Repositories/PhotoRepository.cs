namespace ShiemiApi.Repositories;

public class PhotoRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public Photo? GetById(int id)
        => _context.Photos.Include(u => u.User)
        .Include(d => d.Dev)
        .SingleOrDefault(u => u.Id == id);
    public List<Photo> GetAll()
        => _context.Photos.Include(u => u.User)
        .Include(d => d.Dev)
        .ToList();

    public void Add(Photo photo)
    {
        _context.Photos.Add(photo);
        Save();
    }
    public void Update(Photo photo)
    {
        _context.Photos.Update(photo);
        Save();
    }
    public void Remove(int id)
    {
        var photo = _context.Photos.Single(u => u.Id == id);
        _context.Photos.Remove(photo);
        Save();
    }
    public void Save()
        => _context.SaveChanges();
    public IQueryable<Photo> GetQueryable()
        => _context.Photos;
}
