using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid PetId { get; set; }
    public Guid ProviderId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public Customer Customer { get; set; } = null!;
    public Pet Pet { get; set; } = null!;
    public Provider Provider { get; set; } = null!;
}