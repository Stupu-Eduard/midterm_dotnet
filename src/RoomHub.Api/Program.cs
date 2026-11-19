using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Bookings;
using RoomHub.Api.Features.Rooms;
using RoomHub.Api.Infrastructure.Persistence;
using RoomHub.Api.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Enums as text in JSON.
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Controllable time: handlers read "now" from TimeProvider and pass it to the domain.
builder.Services.AddSingleton(TimeProvider.System);

// EF Core + SQLite (Scoped).
builder.Services.AddDbContext<RoomHubDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RoomHub") ?? "Data Source=roomhub.db"));

// Unit of Work must be the SAME scoped DbContext instance the repositories use.
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RoomHubDbContext>());

// Repositories (interfaces in Domain, implementations in Infrastructure).
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Auto-register every ICommandHandler<,> implementation.
builder.Services.AddHandlers(typeof(Program).Assembly);

var app = builder.Build();

await app.InitializeDatabaseAsync();

app.MapGet("/", () => "RoomHub API is running.");
app.MapRooms();
app.MapBookings();

app.Run();
