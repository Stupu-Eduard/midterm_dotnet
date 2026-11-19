using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.InMemory;

// Still in-memory (lost on restart). Replaced by EF Core + SQLite in level C.
public sealed class RoomRepository
{
    private readonly List<Room> _rooms = new();
    private readonly object _lock = new();

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            return Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));
        }
    }

    public void Add(Room room)
    {
        lock (_lock)
        {
            _rooms.Add(room);
        }
    }
}
