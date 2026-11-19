using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    private Booking() { }

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateRange Period { get; private set; } = null!;
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        IEnumerable<Booking> existingRoomBookings,
        DateTime nowUtc)
    {
        // R2
        if (string.IsNullOrWhiteSpace(bookedBy))
            return BookingErrors.InvalidBookedBy;

        // R3
        var periodResult = DateRange.Create(startUtc, endUtc);
        if (periodResult.IsFailure)
            return periodResult.Error;

        var period = periodResult.Value;

        // R4
        if (attendees <= 0)
            return BookingErrors.InvalidAttendees;

        if (attendees > room.Capacity)
            return BookingErrors.AttendeesExceedCapacity;

        // R5: "now" is passed in by the caller (from TimeProvider).
        if (period.StartUtc < nowUtc)
            return BookingErrors.StartInPast;

        // R6: exactly 4 hours is allowed.
        if (period.Duration > MaxDuration)
            return BookingErrors.TooLong;

        // R7: only active bookings block the interval (a cancelled one frees it, R8).
        if (existingRoomBookings.Any(b => b.Status == BookingStatus.Active && b.Period.Overlaps(period)))
            return BookingErrors.RoomOccupied;

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            Period = period,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    // R8: Active -> Cancelled; cancelling twice is a conflict.
    public Result<Unit> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return BookingErrors.AlreadyCancelled;

        Status = BookingStatus.Cancelled;
        return Unit.Value;
    }
}
