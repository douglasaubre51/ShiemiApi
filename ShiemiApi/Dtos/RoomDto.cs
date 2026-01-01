namespace ShiemiApi.Dtos;

public class RoomDto
{
    public int Id { get; set; }

    public int OwnerId { get; set; }
    public int TenantId { get; set; }
    public int ProjectId { get; set; }

    public List<MessageDto>? Messages { get; set; }
}

public record GetPrivateRoomDto(
    int UserId,
    int ProjectId,
	int DevId,
    RoomTypes RoomType
);
