using Microsoft.EntityFrameworkCore;
using PetCare.Bookings.Domain.Entities;

namespace PetCare.Bookings.Infrastructure.Persistence;

public sealed class DevelopmentDataSeeder(
    ApplicationDbContext dbContext)
{
    public static readonly Guid CustomerId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static readonly Guid PetId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static readonly Guid ProviderId =
        Guid.Parse("33333333-3333-3333-3333-333333333333");

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        if (!await dbContext.Customers.AnyAsync(
                x => x.Id == CustomerId,
                cancellationToken))
        {
            dbContext.Customers.Add(new Customer
            {
                Id = CustomerId,
                Name = "Sample Customer",
                Email = "customer@example.com"
            });
        }

        if (!await dbContext.Pets.AnyAsync(
                x => x.Id == PetId,
                cancellationToken))
        {
            dbContext.Pets.Add(new Pet
            {
                Id = PetId,
                CustomerId = CustomerId,
                Name = "Murphy",
                Type = "Dog"
            });
        }

        if (!await dbContext.Providers.AnyAsync(
                x => x.Id == ProviderId,
                cancellationToken))
        {
            dbContext.Providers.Add(new Provider
            {
                Id = ProviderId,
                Name = "Sample Provider"
            });
        }

        await dbContext.SaveChangesAsync(
            cancellationToken);
    }
}
