using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid PetId { get; set; }
    public Guid ProviderId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public Customer Customer { get; set; } = null!;
    public Pet Pet { get; set; } = null!;
    public Provider Provider { get; set; } = null!;

    public bool Confirm()
    {
        if (Status != BookingStatus.Pending) return false;
        Status = BookingStatus.Confirmed;
        return true;
    }

    public bool Start()
    {
        if (Status != BookingStatus.Confirmed) return false;
        Status = BookingStatus.InProgress;
        return true;
    }

    public bool Complete()
    {
        if (Status != BookingStatus.InProgress) return false;
        Status = BookingStatus.Completed;
        return true;
    }

    public bool Cancel()
    {
        if (Status is not (BookingStatus.Pending or BookingStatus.Confirmed)) return false;
        Status = BookingStatus.Cancelled;
        return true;
    }
}
