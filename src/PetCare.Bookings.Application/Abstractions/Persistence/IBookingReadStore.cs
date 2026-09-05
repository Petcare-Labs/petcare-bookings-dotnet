using PetCare.Bookings.Application.Bookings.Common;
using PetCare.Bookings.Application.Common.Pagination;
using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Abstractions.Persistence;

public interface IBookingReadStore
{
    Task<BookingDetails?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default);

    Task<PagedResult<BookingSummary>> GetAsync(
        Guid? customerId,
        Guid? providerId,
        BookingStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
