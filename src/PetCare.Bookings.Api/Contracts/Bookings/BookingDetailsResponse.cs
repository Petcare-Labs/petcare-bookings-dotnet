using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Api.Contracts.Bookings;

public sealed record BookingDetailsResponse(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    Guid PetId,
    string PetName,
    Guid ProviderId,
    string ProviderName,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    BookingStatus Status);
