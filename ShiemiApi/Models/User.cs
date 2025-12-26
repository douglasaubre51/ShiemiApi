namespace ShiemiApi.Models;

public class User
{
    public int Id { get; set; }
    public Dev? Dev { get; set; }
    public Photo? ProfilePhoto { get; set; }
    public List<Project> Projects { get; set; } = [];

    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public long Phone { get; set; }
    public bool IsDeveloper { get; set; }
    public bool IsAdmin { get; set; }

}