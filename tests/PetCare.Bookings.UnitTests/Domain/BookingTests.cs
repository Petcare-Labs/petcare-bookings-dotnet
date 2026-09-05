using PetCare.Bookings.Domain.Entities;
using PetCare.Bookings.Domain.Enums;

namespace PetCare.Bookings.UnitTests.Domain;

public sealed class BookingTests
{
    [Fact]
    public void Confirm_WhenPending_SetsStatusToConfirmed()
    {
        // Arrange
        var booking = new Booking();

        // Act
        var result = booking.Confirm();

        // Assert
        Assert.True(result);
        Assert.Equal(
            BookingStatus.Confirmed,
            booking.Status);
    }

    [Fact]
    public void Start_WhenPending_DoesNotChangeStatus()
    {
        // Arrange
        var booking = new Booking();

        // Act
        var result = booking.Start();

        // Assert
        Assert.False(result);
        Assert.Equal(
            BookingStatus.Pending,
            booking.Status);
    }

    [Fact]
    public void Start_WhenConfirmed_SetsStatusToInProgress()
    {
        // Arrange
        var booking = new Booking();

        booking.Confirm();

        // Act
        var result = booking.Start();

        // Assert
        Assert.True(result);
        Assert.Equal(
            BookingStatus.InProgress,
            booking.Status);
    }

    [Fact]
    public void Complete_WhenInProgress_SetsStatusToCompleted()
    {
        // Arrange
        var booking = new Booking();

        booking.Confirm();
        booking.Start();

        // Act
        var result = booking.Complete();

        // Assert
        Assert.True(result);
        Assert.Equal(
            BookingStatus.Completed,
            booking.Status);
    }

    [Fact]
    public void Cancel_WhenPending_SetsStatusToCancelled()
    {
        var booking = new Booking();

        var result = booking.Cancel();

        Assert.True(result);
        Assert.Equal(
            BookingStatus.Cancelled,
            booking.Status);
    }

    [Fact]
    public void Cancel_WhenConfirmed_SetsStatusToCancelled()
    {
        var booking = new Booking();

        booking.Confirm();

        var result = booking.Cancel();

        Assert.True(result);
        Assert.Equal(
            BookingStatus.Cancelled,
            booking.Status);
    }

    [Fact]
    public void Cancel_WhenInProgress_DoesNotChangeStatus()
    {
        var booking = new Booking();

        booking.Confirm();
        booking.Start();

        var result = booking.Cancel();

        Assert.False(result);
        Assert.Equal(
            BookingStatus.InProgress,
            booking.Status);
    }

    [Fact]
    public void Confirm_WhenCancelled_Fails()
    {
        var booking = new Booking();

        booking.Cancel();

        var result = booking.Confirm();

        Assert.False(result);
        Assert.Equal(
            BookingStatus.Cancelled,
            booking.Status);
    }

    [Fact]
    public void Complete_WhenAlreadyCompleted_Fails()
    {
        var booking = new Booking();

        booking.Confirm();
        booking.Start();
        booking.Complete();

        var result = booking.Complete();

        Assert.False(result);
        Assert.Equal(
            BookingStatus.Completed,
            booking.Status);
    }
}