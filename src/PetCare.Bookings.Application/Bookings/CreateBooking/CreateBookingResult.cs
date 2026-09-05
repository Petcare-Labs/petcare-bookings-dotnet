using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.CreateBooking;

public sealed record CreateBookingResult(Guid? BookingId, BookingStatus? Status, CreateBookingError? Error)
{
    public bool IsSuccess => Error is null;

    public static CreateBookingResult Success(Guid bookingId, BookingStatus status)
    {
        return new CreateBookingResult(bookingId, status, null);
    }

    public static CreateBookingResult Failure(CreateBookingError error)
    {
        return new CreateBookingResult(null, null, error);
    }
}