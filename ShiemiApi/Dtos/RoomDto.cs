namespace ShiemiApi.Dtos;

public class RoomDto
{
    public int Id { get; set; }

    public int OwnerId { get; set; }
    public User Owner { get; set; }

    public int TenantId { get; set; }
    public User Tenant { get; set; }

    public int ProjectId { get; set; }

    public List<MessageDto>? Messages { get; set; }
}

public record GetPrivateRoomDto(
    int UserId,
    int ProjectId,
    int DevId,
    RoomTypes RoomType
);

public class GetDevRoomDto
{
    public int RoomId { get; set; }
    public int ClientId { get; set; }

    public string ProfilePhotoURL { get; set; }
    public string ClientName { get; set; }
    public RoomTypes RoomType { get; set; } = RoomTypes.DEV;
}

public record GetAllRoomsDto(
        int RoomId,
        int? ProjectId,
        int? OwnerId,
        int? TenantId,
        int? DevId,
        RoomTypes RoomType
);

public record GetAllRoomsWithUsersDto(
        int RoomId,
        string OwnerName,
        string OwnerProfileURL,
        int OwnerId,
        string TenantName,
        string TenantProfileURL,
        int TenantId,
        RoomTypes RoomType
);

public record GetDevProfileDto(
    int RoomId,
    int OwnerId,
    string OwnerName,
    string OwnerProfile,
    int DevId
);