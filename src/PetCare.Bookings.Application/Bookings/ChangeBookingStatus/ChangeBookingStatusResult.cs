using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.ChangeBookingStatus;

public sealed record ChangeBookingStatusResult(Guid? BookingId, BookingStatus? Status, ChangeBookingStatusError? Error)
{
    public bool IsSuccess => Error is null;

    public static ChangeBookingStatusResult Success(Guid bookingId, BookingStatus status)
    {
        return new ChangeBookingStatusResult(bookingId, status, null);
    }

    public static ChangeBookingStatusResult Failure(ChangeBookingStatusError error)
    {
        return new ChangeBookingStatusResult(null, null, error);
    }
}