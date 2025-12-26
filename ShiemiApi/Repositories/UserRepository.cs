namespace ShiemiApi.Repositories;

public class UserRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public void Create(User user)
    {
        _context.Users.Add(user);
        Save();
    }
    public bool Create(UserDto dto)
    {
        var result = _context.Users.Any(u => u.UserId == dto.UserId);
        if (result is false)
        {
            User userDto = new()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserId = dto.UserId,
                Id = dto.Id
            };

            _context.Users.Add(userDto);
            Save();
            return true;
        }
        return false;
    }

    public User? GetById(int id)
        => _context.Users.Include(d => d.Dev)
        .Include(p => p.ProfilePhoto)
        .SingleOrDefault(u => u.Id == id);

    public User? GetByUserId(string id)
        => _context.Users.Include(p => p.ProfilePhoto)
        .SingleOrDefault(u => u.UserId == id);
    public List<User> GetAll()
        => [.. _context.Users];

    public void Update(User user)
    {
        _context.Users.Update(user);
        Save();
    }
    public void Remove(int id)
    {
        var user = _context.Users.Single(u => u.Id == id);
        _context.Users.Remove(user);
        Save();
    }
    public void Save()
        => _context.SaveChanges();
    public IQueryable<User> GetQueryable()
        => _context.Users;
}