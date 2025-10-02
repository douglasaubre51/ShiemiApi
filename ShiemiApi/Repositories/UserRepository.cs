namespace ShiemiApi.Repositories;

public class UserRepository(ApplicationDbContext context)
{

    private readonly ApplicationDbContext _context = context;

    public void Save()
    => _context.SaveChanges();

    // Create

    public void Create(User user)
    {
        _context.Users.
            Add(user);

        Save();
    }

    public bool Create(CreateUserDto dto)
    {
        var result = _context.Users.Any(u => u.UserId == dto.Id);
        if (result is false)
        {
            User userDto = new()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserId = dto.Id
            };

            _context.Users.Add(userDto);
            Save();

            return true;
        }

        return false;
    }

    // Read

    public User GetById(int id)
    => _context.Users
    .Where(u => u.Id == id)
    .SingleOrDefault();

    public User GetByUserId(string id)
    => _context.Users
    .Where(u => u.UserId == id)
    .SingleOrDefault();

    public List<User> GetAll()
    => _context.Users
    .ToList();

    // Update

    public void Update(User user)
    {
        _context.Users
            .Update(user);

        Save();
    }

    // Delete

    public void Remove(int id)
    {
        var user = _context.Users
            .Where(u => u.Id == id)
            .Single();

        _context.Users
            .Remove(user);

        Save();
    }
}
