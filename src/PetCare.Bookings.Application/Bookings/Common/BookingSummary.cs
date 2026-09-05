using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.Common;

public sealed record BookingSummary(
    Guid Id,
    string PetName,
    string ProviderName,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    BookingStatus Status);
