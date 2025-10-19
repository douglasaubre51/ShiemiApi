namespace ShiemiApi.Repositories;

public class ChannelRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public void Save()
    {
        _context.SaveChanges();
    }

    // Create

    public void Create(Channel channel)
    {
        _context.Channels
            .Add(channel);
        Save();
    }

    // Read

    public Channel GetById(int id)
    {
        return _context.Channels
            .Where(u => u.Id == id)
            .SingleOrDefault();
    }

    public List<Channel> GetAll()
    {
        return _context.Channels
            .ToList();
    }

    // Update

    public void Update(Channel channel)
    {
        _context.Channels
            .Update(channel);

        Save();
    }

    // Delete

    public void Remove(int Id)
    {
        var channel = _context.Channels
            .Where(p => p.Id == Id)
            .Single();

        _context.Channels
            .Remove(channel);

        Save();
    }
}