using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Application.Bookings.ChangeBookingStatus;

public sealed class ChangeBookingStatusHandler(IBookingStore bookingStore)
{
    public async Task<ChangeBookingStatusResult> HandleAsync(ChangeBookingStatusCommand command, CancellationToken cancellationToken = default)
    {
        var booking = await bookingStore.GetByIdAsync(command.BookingId, cancellationToken);

        if (booking is null) return ChangeBookingStatusResult.Failure(ChangeBookingStatusError.BookingNotFound);

        var transitionSucceeded =
            command.TargetStatus switch
            {
                BookingStatus.Confirmed =>
                    booking.Confirm(),

                BookingStatus.InProgress =>
                    booking.Start(),

                BookingStatus.Completed =>
                    booking.Complete(),

                BookingStatus.Cancelled =>
                    booking.Cancel(),

                _ => false
            };

        if (!transitionSucceeded) return ChangeBookingStatusResult.Failure(ChangeBookingStatusError.InvalidTransition);

        await bookingStore.SaveChangesAsync(cancellationToken);

        return ChangeBookingStatusResult.Success(booking.Id, booking.Status);
    }
}