using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Bookings.Domain.Entities;

namespace PetCare.Bookings.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasMany(x => x.Pets)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId);
    }
}
