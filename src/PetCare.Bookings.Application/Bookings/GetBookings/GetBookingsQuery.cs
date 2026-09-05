using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.GetBookings;

public sealed record GetBookingsQuery(Guid? CustomerId, Guid? ProviderId, BookingStatus? Status);