using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Bookings.Application.Abstractions.Persistence;
using PetCare.Bookings.Infrastructure.Persistence;

namespace PetCare.Bookings.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Database") ?? throw new InvalidOperationException(
                "Database connection string is not configured.");

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IBookingStore, BookingStore>();
        services.AddScoped<DevelopmentDataSeeder>();
        return services;
    }
}