using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetCare.Bookings.Api.Contracts.Bookings;
using PetCare.Bookings.Application.Bookings.ChangeBookingStatus;
using PetCare.Bookings.Application.Bookings.CreateBooking;
using PetCare.Bookings.Domain.Enums;

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

    [HttpPost("{id:guid}/confirm")]
    public Task<ActionResult<BookingStatusResponse>> ConfirmAsync(Guid id, ChangeBookingStatusHandler handler, CancellationToken cancellationToken)
    {
        return ChangeStatusAsync(
            id,
            BookingStatus.Confirmed,
            handler,
            cancellationToken);
    }

    [HttpPost("{id:guid}/start")]
    public Task<ActionResult<BookingStatusResponse>> StartAsync(Guid id, ChangeBookingStatusHandler handler, CancellationToken cancellationToken)
    {
        return ChangeStatusAsync(
            id,
            BookingStatus.InProgress,
            handler,
            cancellationToken);
    }

    [HttpPost("{id:guid}/complete")]
    public Task<ActionResult<BookingStatusResponse>> CompleteAsync(Guid id, ChangeBookingStatusHandler handler, CancellationToken cancellationToken)
    {
        return ChangeStatusAsync(
            id,
            BookingStatus.Completed,
            handler,
            cancellationToken);
    }

    [HttpPost("{id:guid}/cancel")]
    public Task<ActionResult<BookingStatusResponse>> CancelAsync(Guid id, ChangeBookingStatusHandler handler, CancellationToken cancellationToken)
    {
        return ChangeStatusAsync(
            id,
            BookingStatus.Cancelled,
            handler,
            cancellationToken);
    }


    private async Task<ActionResult<BookingStatusResponse>> ChangeStatusAsync(Guid bookingId, BookingStatus targetStatus, ChangeBookingStatusHandler handler, CancellationToken cancellationToken)
    {
        var command = new ChangeBookingStatusCommand(bookingId, targetStatus);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error switch
            {
                ChangeBookingStatusError.BookingNotFound =>
                    NotFound("Booking was not found."),

                ChangeBookingStatusError.InvalidTransition =>
                    Conflict(
                        $"Booking cannot transition to {targetStatus}."),

                _ => Problem()
            };
        }

        return Ok(new BookingStatusResponse(result.BookingId!.Value, result.Status!.Value));
    }
}