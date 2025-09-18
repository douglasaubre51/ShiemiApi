namespace ShiemiApi.Repositories
{

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

        // Read

        public User GetById(int id)
            => _context.Users
            .Where(u => u.Id == id)
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

        public void Delete(User user)
        {
            _context.Users
            .Remove(user);

            Save();
        }
    }
}

