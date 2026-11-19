using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.InMemory;

// Still in-memory (lost on restart). Replaced by EF Core + SQLite in level C.
public sealed class BookingRepository
{
    private readonly List<Booking> _bookings = new();
    private readonly object _lock = new();

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            return Task.FromResult(_bookings.FirstOrDefault(b => b.Id == id));
        }
    }

    public Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            IReadOnlyList<Booking> result = _bookings
                .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
                .ToList();
            return Task.FromResult(result);
        }
    }

    // R9: active bookings of one participant, across all rooms.
    public Task<IReadOnlyList<Booking>> GetActiveByBookedByAsync(string bookedBy, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            IReadOnlyList<Booking> result = _bookings
                .Where(b => b.BookedBy == bookedBy && b.Status == BookingStatus.Active)
                .ToList();
            return Task.FromResult(result);
        }
    }

    public void Add(Booking booking)
    {
        lock (_lock)
        {
            _bookings.Add(booking);
        }
    }
}
