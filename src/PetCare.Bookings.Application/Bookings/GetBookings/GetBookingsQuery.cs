using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.GetBookings;

public sealed record GetBookingsQuery(Guid? CustomerId, Guid? ProviderId, BookingStatus? Status, int PageNumber = 1, int pageSize = 20);
