namespace ShiemiApi.Models;

public class User
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Dev? Dev { get; set; }
    public Photo? ProfilePhoto { get; set; }
    public List<Project> Projects { get; set; } = [];
    public List<int>? PastProjects { get; set; } = [];

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Optional
    public string Contact { get; set; } = string.Empty;
    public string Whatsaap { get; set; } = string.Empty;
    public string LinkedIn { get; set; } = string.Empty;
    public string Gmail { get; set; } = string.Empty;
    public string Github { get; set; } = string.Empty;
    public string AboutMe { get; set; } = string.Empty;

    public bool IsDeveloper { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBanned { get; set; }
}