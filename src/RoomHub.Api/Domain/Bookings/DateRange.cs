using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public sealed record DateRange
{
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }

    private DateRange() { }

    private DateRange(DateTime startUtc, DateTime endUtc)
    {
        StartUtc = startUtc;
        EndUtc = endUtc;
    }

    public TimeSpan Duration => EndUtc - StartUtc;

    // R3: end strictly after start.
    public static Result<DateRange> Create(DateTime start, DateTime end)
    {
        var startUtc = ToUtc(start);
        var endUtc = ToUtc(end);

        if (endUtc <= startUtc)
            return DateRangeErrors.EndNotAfterStart;

        return new DateRange(startUtc, endUtc);
    }

    // R7: strict comparisons, so touching intervals (one ends when the other starts) do not overlap.
    public bool Overlaps(DateRange other) =>
        StartUtc < other.EndUtc && other.StartUtc < EndUtc;

    private static DateTime ToUtc(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
}
