namespace PetCare.Bookings.Domain.Entities;

public class Provider
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Booking> Bookings { get; set; } = [];
}
