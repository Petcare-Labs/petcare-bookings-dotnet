using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Application.Bookings.Common;
using PetCare.Bookings.Application.Common.Pagination;

namespace PetCare.Bookings.Application.Bookings.GetBookings;

public sealed class GetBookingsHandler(IBookingReadStore bookingReadStore)
{
    public Task<PagedResult<BookingSummary>> HandleAsync(GetBookingsQuery query, CancellationToken cancellationToken = default)
    {
        return bookingReadStore.GetAsync(
            query.CustomerId,
            query.ProviderId,
            query.Status,
            query.PageNumber,
            query.PageSize,
            cancellationToken);
    }
}
