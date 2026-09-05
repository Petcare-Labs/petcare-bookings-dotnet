using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Bookings.Application.Bookings.ChangeBookingStatus;
using PetCare.Bookings.Application.Bookings.CreateBooking;
using PetCare.Bookings.Application.Bookings.GetBookingById;
using PetCare.Bookings.Application.Bookings.GetBookings;

namespace PetCare.Bookings.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreateBookingHandler>();
        services.AddScoped<ChangeBookingStatusHandler>();
        services.AddScoped<CreateBookingHandler>();
        services.AddScoped<GetBookingByIdHandler>();
        services.AddScoped<GetBookingsHandler>();

        services.AddScoped<IValidator<CreateBookingCommand>, CreateBookingValidator>();

        return services;
    }
}
