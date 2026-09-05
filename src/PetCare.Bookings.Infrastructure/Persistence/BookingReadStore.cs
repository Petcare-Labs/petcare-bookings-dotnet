using Microsoft.EntityFrameworkCore;
using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Application.Bookings.Common;
using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Infrastructure.Persistence;

public sealed class BookingReadStore(
    ApplicationDbContext dbContext)
    : IBookingReadStore
{
    public Task<BookingDetails?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        return dbContext.Bookings
            .AsNoTracking()
            .Where(x => x.Id == bookingId)
            .Select(x => new BookingDetails(
                x.Id,
                x.CustomerId,
                x.Customer.Name,
                x.PetId,
                x.Pet.Name,
                x.ProviderId,
                x.Provider.Name,
                x.StartTime,
                x.EndTime,
                x.Status))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BookingSummary>> GetAsync(Guid? customerId = null, Guid? providerId = null,
        BookingStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Bookings
            .AsNoTracking()
            .AsQueryable();

        if (customerId.HasValue)
        {
            query = query.Where(x => x.CustomerId == customerId.Value);
        }

        if (providerId.HasValue)
        {
            query = query.Where(x => x.ProviderId == providerId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return await query
            .OrderBy(x => x.StartTime)
            .Select(x => new BookingSummary(
                x.Id,
                x.Pet.Name,
                x.Provider.Name,
                x.StartTime,
                x.EndTime,
                x.Status))
            .ToListAsync(cancellationToken);
    }
}