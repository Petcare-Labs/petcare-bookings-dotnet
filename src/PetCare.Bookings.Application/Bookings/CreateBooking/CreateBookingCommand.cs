namespace PetCare.Bookings.Application.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid CustomerId,
    Guid PetId,
    Guid ProviderId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime
);
