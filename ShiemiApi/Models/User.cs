namespace ShiemiApi.Models;

public class User
{
    public int Id { get; set; }
    public string UserId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Profile { get; set; }
    public string Email { get; set; }
    public long? Phone { get; set; }

    public bool IsDeveloper { get; set; }
    public bool IsAdmin { get; set; }

    public List<Project>? Projects { get; set; }
}
