namespace RoomHub.Api.Domain.Rooms;

// Level A: still anemic. Becomes a rich entity in level B.
public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
}
