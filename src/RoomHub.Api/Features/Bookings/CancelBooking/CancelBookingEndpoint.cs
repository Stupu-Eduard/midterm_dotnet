using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Infrastructure.InMemory;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/{id:guid}/cancel", Handle)
            .WithName("CancelBooking");

    private static NoContent Handle(Guid id, BookingRepository bookings)
    {
        var booking = bookings.Get(id)
            ?? throw new BusinessException("BookingNotFound", "The booking does not exist.", 404);

        booking.Status = BookingStatus.Cancelled;

        return TypedResults.NoContent();
    }
}
