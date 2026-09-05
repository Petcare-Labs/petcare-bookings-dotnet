using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Application.Bookings.Common;

namespace PetCare.Bookings.Application.Bookings.GetBookings;

public sealed class GetBookingsHandler(IBookingReadStore bookingReadStore)
{
    public Task<IReadOnlyList<BookingSummary>> HandleAsync(GetBookingsQuery query, CancellationToken cancellationToken = default)
    {
        return bookingReadStore.GetAsync(
            query.CustomerId,
            query.ProviderId,
            query.Status,
            cancellationToken);
    }
}
