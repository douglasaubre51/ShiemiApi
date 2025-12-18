namespace ShiemiApi.Dtos;

public record UserDto(
    int Id,
    string UserId,
    string FirstName,
    string LastName,
    string Email,
    bool IsDeveloper
);

public record CreateUserDto(
    string Id,
    string FirstName,
    string LastName,
    string Email
);

public class DevUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Profile { get; set; } = string.Empty;
    public bool IsDeveloper { get; set; }
    public long Phone { get; set; }
}