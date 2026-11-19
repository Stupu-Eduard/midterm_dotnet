using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
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

// Framework errors (malformed JSON, missing body) are also returned as ProblemDetails.
builder.Services.AddProblemDetails();

// EF Core + SQLite (Scoped). A relative database path is anchored to the content root,
// so the same file is used no matter which folder the app is started from.
var sqlite = new SqliteConnectionStringBuilder(
    builder.Configuration.GetConnectionString("RoomHub") ?? "Data Source=roomhub.db");
if (!Path.IsPathRooted(sqlite.DataSource))
    sqlite.DataSource = Path.Combine(builder.Environment.ContentRootPath, sqlite.DataSource);

builder.Services.AddDbContext<RoomHubDbContext>(options =>
    options.UseSqlite(sqlite.ToString()));

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
