namespace ShiemiApi.Repositories;

public class RoomRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public IQueryable<Room> GetQueryable()
        => _context.Rooms;
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
    public List<MessageDto>? GetAllMessagesByRoomId(int id, RoomTypes roomType)
    {
        var messages = _context.Rooms
            .Include(m => m.Messages)
            .ThenInclude(message => message.User)
            .Where(r => r.Id == id)
            .Single(r => r.RoomType == roomType)
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

    private void Save()
        => _context.SaveChanges();
    public void Create(Room room)
    {
        _context.Rooms.Add(room);
        Save();
    }
    public void AddMessage(int roomId, Message message)
    {
        _context.Rooms.Include(m => m.Messages)
            .Single(r => r.Id == roomId)
            .Messages!.Add(message);
        Save();
    }
    public void Add(Room room)
    {
        _context.Rooms.Add(room);
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
    public void RemoveAll()
    {
        _context.Rooms.ExecuteDelete();
    }
}
