namespace PetCare.Bookings.Api.Contracts.Bookings;

public sealed record CreateBookingRequest(
    Guid CustomerId,
    Guid PetId,
    Guid ProviderId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
