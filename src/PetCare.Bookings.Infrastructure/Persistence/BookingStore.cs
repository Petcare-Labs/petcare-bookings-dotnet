using Microsoft.EntityFrameworkCore;
using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Domain.Entities;
using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Infrastructure.Persistence;

public sealed class BookingStore(ApplicationDbContext dbContext) : IBookingStore
{
    public Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return dbContext.Customers
            .AnyAsync(
                x => x.Id == customerId,
                cancellationToken);
    }

    public Task<bool> PetBelongsToCustomerAsync(Guid petId, Guid customerId, CancellationToken cancellationToken = default)
    {
        return dbContext.Pets
            .AnyAsync(
                x => x.Id == petId &&
                     x.CustomerId == customerId,
                cancellationToken);
    }

    public Task<bool> ProviderExistsAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        return dbContext.Providers
            .AnyAsync(
                x => x.Id == providerId,
                cancellationToken);
    }

    public Task<bool> HasOverlappingBookingAsync(Guid providerId, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default)
    {
        return dbContext.Bookings.AnyAsync(
            x =>
                x.ProviderId == providerId &&
                x.Status != BookingStatus.Cancelled &&
                x.StartTime < endTime &&
                x.EndTime > startTime,
            cancellationToken);
    }

    public Task<Booking?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        return dbContext.Bookings
            .FirstOrDefaultAsync(
                x => x.Id == bookingId,
                cancellationToken);
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await dbContext.Bookings.AddAsync(booking, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
