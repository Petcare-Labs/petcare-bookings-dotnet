using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Api.Contracts.Bookings;

public sealed record BookingSummaryResponse(
    Guid Id,
    string PetName,
    string ProviderName,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    BookingStatus Status);
