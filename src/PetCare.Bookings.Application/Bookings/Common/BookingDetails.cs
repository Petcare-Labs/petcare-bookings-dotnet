using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.Common;

public sealed record BookingDetails(
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