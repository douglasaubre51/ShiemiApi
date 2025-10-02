namespace ShiemiApi.Models
{

    public class Room
    {

        public int Id { get; set; }

        public User Owner { get; set; }
        public User Tenant { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public List<Message> Messages { get; set; }
    }
}
