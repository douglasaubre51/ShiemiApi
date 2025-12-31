namespace ShiemiApi.Dtos.HubDtos;

public class DevRoomDto
{
	public int RoomId { get; set; }
	public int DevId { get; set; }

	public int OwnerId { get; set; }
	public int TenantId { get; set; }

	public List<MessageDto> Messages { get; set; } = [];
}
