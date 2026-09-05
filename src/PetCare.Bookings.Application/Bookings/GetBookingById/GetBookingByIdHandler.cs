using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Application.Bookings.Common;

namespace PetCare.Bookings.Application.Bookings.GetBookingById;

public sealed class GetBookingByIdHandler(
    IBookingReadStore bookingReadStore)
{
    public Task<BookingDetails?> HandleAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        return bookingReadStore.GetByIdAsync(bookingId, cancellationToken);
    }
}