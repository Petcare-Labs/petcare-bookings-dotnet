using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Domain.Entities;
using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.CreateBooking;

public sealed class CreateBookingHandler(IBookingStore bookingStore)
{
    public async Task<CreateBookingResult> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        var customerExists = await bookingStore.CustomerExistsAsync(command.CustomerId, cancellationToken);

        if (!customerExists) return CreateBookingResult.Failure(CreateBookingError.CustomerNotFound);

        var petBelongsToCustomer = await bookingStore.PetBelongsToCustomerAsync(command.PetId, command.CustomerId, cancellationToken);

        if (!petBelongsToCustomer) return CreateBookingResult.Failure(CreateBookingError.PetNotFoundOrDoesNotBelongToCustomer);

        var providerExists = await bookingStore.ProviderExistsAsync(command.ProviderId, cancellationToken);

        if (!providerExists) return CreateBookingResult.Failure(CreateBookingError.ProviderNotFound);

        var hasOverlap = await bookingStore.HasOverlappingBookingAsync(command.ProviderId, command.StartTime, command.EndTime, cancellationToken);

        if (hasOverlap) return CreateBookingResult.Failure(CreateBookingError.ProviderHasOverlappingBooking);

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            CustomerId = command.CustomerId,
            PetId = command.PetId,
            ProviderId = command.ProviderId,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            Status = BookingStatus.Pending
        };

        await bookingStore.AddAsync(booking, cancellationToken);

        await bookingStore.SaveChangesAsync(cancellationToken);

        return CreateBookingResult.Success(booking.Id, booking.Status);
    }
}