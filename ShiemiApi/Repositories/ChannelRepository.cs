namespace ShiemiApi.Repositories;

public class ChannelRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public void Save()
        => _context.SaveChanges();

    public void Create(Channel channel)
    {
        _context.Channels.Add(channel);
        Save();
    }

    public Channel? GetById(int id)
        => _context.Channels.Include(p => p.Project)
        .Include(m => m.Messages)!
        .ThenInclude(u => u.User)
        .SingleOrDefault(c => c.Id == id);
    public List<Channel> GetAll()
         => [.. _context.Channels];

    public void AddMessage(int channelId, Message message)
    {
        _context.Channels.SingleOrDefault(c => c.Id == channelId)!
            .Messages!
            .Add(message);
        _context.SaveChanges();
    }

    public void Update(Channel channel)
    {
        _context.Channels.Update(channel);
        Save();
    }

    public void Remove(int Id)
    {
        var channel = _context.Channels.Single(c => c.Id == Id);
        _context.Channels.Remove(channel);
        Save();
    }
}