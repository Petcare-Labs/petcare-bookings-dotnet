using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetCare.Bookings.Api.Contracts.Bookings;
using PetCare.Bookings.Application.Bookings.CreateBooking;

namespace PetCare.Bookings.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public sealed class BookingsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateBookingResponse>> CreateAsync(
        CreateBookingRequest request,
        CreateBookingHandler handler,
        IValidator<CreateBookingCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand(
            request.CustomerId,
            request.PetId,
            request.ProviderId,
            request.StartTime,
            request.EndTime);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return ValidationProblem(
                new ValidationProblemDetails(
                    validationResult.ToDictionary()));
        }

        var result = await handler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error switch
            {
                CreateBookingError.CustomerNotFound =>
                    NotFound("Customer was not found."),

                CreateBookingError.ProviderNotFound =>
                    NotFound("Provider was not found."),

                CreateBookingError.PetNotFoundOrDoesNotBelongToCustomer =>
                    BadRequest(
                        "Pet was not found or does not belong to the customer."),

                CreateBookingError.ProviderHasOverlappingBooking =>
                    Conflict(
                        "The provider already has a booking during the requested time."),
                        
                _ => Problem()
            };
        }

        var response = new CreateBookingResponse(result.BookingId!.Value, result.Status!.Value);

        return Created($"/api/bookings/{response.Id}", response);
    }
}