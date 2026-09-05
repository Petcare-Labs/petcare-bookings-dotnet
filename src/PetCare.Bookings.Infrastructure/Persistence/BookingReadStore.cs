using Microsoft.EntityFrameworkCore;
using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Application.Bookings.Common;
using PetCare.Bookings.Application.Common.Pagination;
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

    public async Task<PagedResult<BookingSummary>> GetAsync(Guid? customerId, Guid? providerId,
        BookingStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
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

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x => x.StartTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new BookingSummary(
                x.Id,
                x.Pet.Name,
                x.Provider.Name,
                x.StartTime,
                x.EndTime,
                x.Status))
            .ToListAsync(cancellationToken);

        return new PagedResult<BookingSummary>(items, pageNumber, pageSize, totalCount);
    }
}