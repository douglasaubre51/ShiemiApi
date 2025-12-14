namespace ShiemiApi.Repositories;

public class ReviewRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public Review? GetById(int id)
        => _context.Reviews.Include(u => u.User)
            .Include(p => p.Project)
            .SingleOrDefault(r => r.Id == id);
    public List<Review> GetAll()
        => _context.Reviews.Include(u => u.User)
            .Include(p => p.Project)
            .ToList();
    public List<Review> GetAllByProjectId(int projectId)
        => _context.Reviews.Include(u => u.User)
        .Include(p => p.Project)
        .Where(e => e.Project!.Id == projectId)
        .ToList();

    public void Create(Review review)
    {
        _context.Reviews.Add(review);
        Save();
    }
    public void Update(Review review)
    {
        _context.Reviews.Update(review);
        Save();
    }
    public void Remove(int Id)
    {
        var review = _context.Reviews.Single(r => r.Id == Id);
        _context.Reviews.Remove(review);
        Save();
    }
    public void Save()
        => _context.SaveChanges();
    public IQueryable<Review> GetQueryable()
        => _context.Reviews;
}
