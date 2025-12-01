namespace ShiemiApi.Dtos;

public record UserDto(
    int Id,
    string UserId,
    string FirstName,
    string LastName,
    string Email
);

public record CreateUserDto(
    string Id,
    string FirstName,
    string LastName,
    string Email
);