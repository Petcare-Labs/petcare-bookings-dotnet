using FluentValidation;

namespace PetCare.Bookings.Application.Bookings.CreateBooking;

public sealed class CreateBookingValidator
    : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.PetId)
            .NotEmpty();

        RuleFor(x => x.ProviderId)
            .NotEmpty();

        RuleFor(x => x.StartTime)
            .NotEmpty();

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time.");
    }
}
