using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetCare.Bookings.Api.Contracts.Bookings;
using PetCare.Bookings.Application.Bookings.ChangeBookingStatus;
using PetCare.Bookings.Application.Bookings.CreateBooking;
using PetCare.Bookings.Application.Bookings.GetBookingById;
using PetCare.Bookings.Application.Bookings.GetBookings;
using PetCare.Bookings.Application.Common.Pagination;
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
                    Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Customer not found",
                        detail: "The requested customer does not exist."),

                CreateBookingError.ProviderNotFound =>
                    Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Provider not found",
                        detail: "The requested provider does not exist."),

                CreateBookingError.PetNotFoundOrDoesNotBelongToCustomer =>
                    Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "Invalid pet",
                        detail: "The pet does not exist or does not belong to the customer."),

                CreateBookingError.ProviderHasOverlappingBooking =>
                    Problem(
                        statusCode: StatusCodes.Status409Conflict,
                        title: "Booking conflict",
                        detail: "The provider already has a booking during the requested time."),

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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookingDetailsResponse>> GetByIdAsync(
        Guid id,
        GetBookingByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var booking = await handler.HandleAsync(id, cancellationToken);

        if (booking is null) return NotFound();

        return Ok(
            new BookingDetailsResponse(
                booking.Id,
                booking.CustomerId,
                booking.CustomerName,
                booking.PetId,
                booking.PetName,
                booking.ProviderId,
                booking.ProviderName,
                booking.StartTime,
                booking.EndTime,
                booking.Status));
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<BookingSummaryResponse>>> GetAsync(
        [FromQuery] Guid? customerId,
        [FromQuery] Guid? providerId,
        [FromQuery] BookingStatus? status,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        GetBookingsHandler handler,
        CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = new GetBookingsQuery(customerId, providerId, status, pageNumber, pageSize);

        var bookings = await handler.HandleAsync(query, cancellationToken);

        var items = bookings.Items
            .Select(x => new BookingSummaryResponse(
                x.Id,
                x.PetName,
                x.ProviderName,
                x.StartTime,
                x.EndTime,
                x.Status))
            .ToList();

        var response = new PagedResult<BookingSummaryResponse>(items, bookings.PageNumber, bookings.PageSize, bookings.TotalCount);


        return Ok(response);
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
