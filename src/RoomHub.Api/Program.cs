using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Features.Bookings;
using RoomHub.Api.Features.Rooms;
using RoomHub.Api.Infrastructure.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Controllable time: handlers read "now" from TimeProvider and pass it to the domain.
builder.Services.AddSingleton(TimeProvider.System);

// In-memory stores (replaced by EF Core + SQLite in level C).
builder.Services.AddSingleton<RoomRepository>();
builder.Services.AddSingleton<BookingRepository>();

// Auto-register every ICommandHandler<,> implementation.
builder.Services.AddHandlers(typeof(Program).Assembly);

var app = builder.Build();

app.MapGet("/", () => "RoomHub API is running.");
app.MapRooms();
app.MapBookings();

app.Run();
