namespace ShiemiApi.Repositories
{
    public class MessageRepository(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public void Save()
            => _context.SaveChanges();

        // Create

        public void Create(Message message)
        {
            _context.Messages
            .Add(message);
            Save();
        }

        // Read

        public Message GetById(int id)
            => _context.Messages
            .Where(u => u.Id == id)
            .SingleOrDefault();

        public List<Message> GetAll()
            => _context.Messages
            .ToList();

		public List<Message> GetAllByChannelId(int channelId)
			=> _context.Messages
			.Where(m => m.Channel.Id == channelId)
			.ToList();

        // Update

        public void Update(Message message)
        {
            _context.Messages
            .Update(message);

            Save();
        }

        // Delete

        public void Remove(int Id)
        {
            var message = _context.Messages
            .Where(p => p.Id == Id)
			.Single();
			
			_context.Messages
			.Remove(message);

            Save();
        }
    }
}

