namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // R7: active bookings of one room (overlap check).
    Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken);

    // R9: active bookings of one participant, across all rooms.
    Task<IReadOnlyList<Booking>> GetActiveByBookedByAsync(string bookedBy, CancellationToken cancellationToken);

    void Add(Booking booking);
}
