using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class DateRangeErrors
{
    public static readonly Error EndNotAfterStart =
        Error.Validation("DateRange.EndNotAfterStart", "The end must be strictly after the start.");
}
