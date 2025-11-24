namespace ShiemiApi.Dtos;

public record UserDto(
    int Id,
    string UserId,
    string FirstName,
    string LastName,
    string Email
);