namespace ShiemiApi.Repositories;

public class RoomRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    private void Save()
        => _context.SaveChanges();

    public void Create(Room room)
    {
        _context.Rooms.Add(room);
        Save();
    }

    public Room? GetById(int id)
        => _context.Rooms.SingleOrDefault(r => r.Id == id);
    public List<Room> GetAll()
        => [.. _context.Rooms];
    public List<Room> GetAllByUserId(int id)
        => _context.Rooms
            .Include(u => u.Owner)
            .Include(u => u.Tenant)
            .Include(m => m.Messages)
            .Where(u => u.Owner.Id == id)
            .ToList();
    public List<MessageDto>? GetAllMessagesByRoomId(int id)
    {
        var messages = _context.Rooms
            .Include(m => m.Messages)!.ThenInclude(message => message.User)
            .Single(r => r.Id == id)
            .Messages;
        if (messages is null)
            return null;

        List<MessageDto> dtoCollection = new();
        foreach (var m in messages!)
        {
            MessageDto dto = new()
            {
                Text = m.Text,
                UserId = m.User.Id,
                RoomId = m.Id,
                CreatedAt = m.CreatedAt
            };
            dtoCollection.Add(dto);
        }

        return dtoCollection;
    }

    public void AddMessage(int roomId, Message message)
    {
        _context.Rooms.Include(m => m.Messages)
            .Single(r => r.Id == roomId)
            .Messages!.Add(message);
        Save();
    }

    public void Update(Room room)
    {
        _context.Rooms.Update(room);
        Save();
    }

    public void Remove(int id)
    {
        var room = _context.Rooms.Single(p => p.Id == id);
        _context.Rooms.Remove(room);
        Save();
    }
}