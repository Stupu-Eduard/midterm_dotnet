using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Infrastructure.InMemory;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

// Read slice: no command handler (reads are thin endpoints in lectures 1-6).
public static class GetBookingByIdEndpoint
{
    public sealed record Response(
        Guid Id,
        Guid RoomId,
        string BookedBy,
        DateTime StartUtc,
        DateTime EndUtc,
        int Attendees,
        string Status);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetBookingById");

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> Handle(
        Guid id,
        BookingRepository bookings,
        CancellationToken ct)
    {
        var b = await bookings.GetByIdAsync(id, ct);
        if (b is null)
            return BookingErrors.NotFound.ToProblem();

        return TypedResults.Ok(new Response(
            b.Id,
            b.RoomId,
            b.BookedBy,
            b.Period.StartUtc,
            b.Period.EndUtc,
            b.Attendees,
            b.Status.ToString()));
    }
}
