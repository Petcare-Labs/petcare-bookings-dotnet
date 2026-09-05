namespace PetCare.Bookings.Domain.Entities;

public class Pet
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Customer Customer { get; set; } = null!;
}
