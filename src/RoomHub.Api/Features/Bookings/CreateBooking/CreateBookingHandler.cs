using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure.InMemory;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees) : ICommand<CreateBookingResult>;

public sealed record CreateBookingResult(Guid Id, Guid RoomId, BookingStatus Status);

public sealed class CreateBookingHandler(
    RoomRepository rooms,
    BookingRepository bookings,
    TimeProvider time)
    : ICommandHandler<CreateBookingCommand, CreateBookingResult>
{
    public async Task<Result<CreateBookingResult>> Handle(
        CreateBookingCommand command,
        CancellationToken cancellationToken)
    {
        // Missing room -> 404 before any validation.
        var room = await rooms.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
            return RoomErrors.NotFound;

        var activeBookings = await bookings.GetActiveForRoomAsync(room.Id, cancellationToken);

        var result = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            activeBookings,
            time.GetUtcNow().UtcDateTime);

        if (result.IsFailure)
            return result.Error;

        var booking = result.Value;
        bookings.Add(booking);

        return new CreateBookingResult(booking.Id, booking.RoomId, booking.Status);
    }
}
