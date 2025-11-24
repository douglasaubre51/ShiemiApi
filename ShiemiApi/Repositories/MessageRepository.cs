namespace ShiemiApi.Repositories;

public class MessageRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public void Save() => _context.SaveChanges();

    public void Create(Message message)
    {
        _context.Messages.Add(message);
        Save();
    }
    public Message GetById(int id)
        => _context.Messages.Single(u => u.Id == id);
    public List<Message> GetAll()
        => [.. _context.Messages];
    public List<Message> GetAllByChannelId(int channelId)
        => _context.Messages.Include(c=>c.Channel)
        .Where(m => m.Channel!.Id == channelId)
        .ToList();

    public void Update(Message message)
    {
        _context.Messages.Update(message);
        Save();
    }

    public void Remove(int Id)
    {
        var message = _context.Messages.Single(p => p.Id == Id);
        _context.Messages.Remove(message);
        Save();
    }
}