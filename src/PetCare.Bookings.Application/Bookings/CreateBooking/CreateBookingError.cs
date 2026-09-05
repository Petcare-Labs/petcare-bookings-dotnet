namespace PetCare.Bookings.Application.Bookings.CreateBooking;

public enum CreateBookingError
{
    CustomerNotFound,
    PetNotFoundOrDoesNotBelongToCustomer,
    ProviderNotFound,
    ProviderHasOverlappingBooking
}
