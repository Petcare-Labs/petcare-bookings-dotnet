using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Api.Contracts.Bookings;

public sealed record CreateBookingResponse(
    Guid Id,
    BookingStatus Status);
