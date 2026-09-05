using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.ChangeBookingStatus;

public sealed record ChangeBookingStatusCommand(Guid BookingId, BookingStatus TargetStatus);
