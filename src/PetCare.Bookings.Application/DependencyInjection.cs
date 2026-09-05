using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Bookings.Application.Bookings.CreateBooking;

namespace PetCare.Bookings.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreateBookingHandler>();

        services.AddScoped<IValidator<CreateBookingCommand>, CreateBookingValidator>();

        return services;
    }
}