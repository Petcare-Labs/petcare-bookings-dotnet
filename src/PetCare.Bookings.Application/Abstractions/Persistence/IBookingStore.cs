using PetCare.Bookings.Domain.Entities;

namespace PetCare.Bookings.Application.Abstractions.Persistence;

public interface IBookingStore
{
    Task<bool> CustomerExistsAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<bool> PetBelongsToCustomerAsync(Guid petId, Guid customerId, CancellationToken cancellationToken = default);
    Task<bool> ProviderExistsAsync(Guid providerId, CancellationToken cancellationToken = default);
    Task<bool> HasOverlappingBookingAsync(Guid providerId, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);
    
    Task<Booking?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default);
    
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}