using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Api.Contracts.Bookings;

public sealed record BookingStatusResponse(Guid Id, BookingStatus Status);
