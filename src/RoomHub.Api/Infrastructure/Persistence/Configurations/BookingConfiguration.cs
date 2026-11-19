using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Property(b => b.BookedBy).IsRequired();
        builder.Property(b => b.Attendees).IsRequired();

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // DateRange value object stored in the same table.
        builder.ComplexProperty(b => b.Period, period =>
        {
            period.Property(p => p.StartUtc)
                .HasColumnName("StartUtc")
                .HasConversion<UtcDateTimeConverter>();

            period.Property(p => p.EndUtc)
                .HasColumnName("EndUtc")
                .HasConversion<UtcDateTimeConverter>();
        });

        builder.HasOne<Room>()
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.RoomId);
        builder.HasIndex(b => b.BookedBy);
    }
}
