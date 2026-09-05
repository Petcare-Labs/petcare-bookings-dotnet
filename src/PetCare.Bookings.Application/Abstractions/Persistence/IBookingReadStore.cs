using PetCare.Bookings.Application.Bookings.Common;
using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Abstractions.Persistence;

public interface IBookingReadStore
{
    Task<BookingDetails?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BookingSummary>> GetAsync(Guid? customerId = null, Guid? providerId = null, BookingStatus? status = null, CancellationToken cancellationToken = default);
}