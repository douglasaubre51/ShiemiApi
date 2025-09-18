namespace ShiemiApi.Data
{

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options)
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Message> Message { get; set; }
    }
}
