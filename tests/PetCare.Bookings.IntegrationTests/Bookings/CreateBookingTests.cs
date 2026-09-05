using System.Net;
using System.Net.Http.Json;
using PetCare.Bookings.Domain.Entities;
using PetCare.Bookings.IntegrationTests.Infrastructure;

namespace PetCare.Bookings.IntegrationTests.Bookings;

public sealed class CreateBookingTests :
    IClassFixture<PostgresFixture>,
    IDisposable
{
    private readonly PetCareWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CreateBookingTests(
        PostgresFixture postgres)
    {
        _factory = new PetCareWebApplicationFactory(postgres.ConnectionString);
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateBooking_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();

        var customerId = Guid.NewGuid();
        var petId = Guid.NewGuid();
        var providerId = Guid.NewGuid();

        await _factory.ExecuteDbContextAsync(async dbContext =>
        {
            dbContext.Customers.Add(new Customer
            {
                Id = customerId,
                Name = "Test Customer",
                Email = $"{customerId}@example.com"
            });

            dbContext.Pets.Add(new Pet
            {
                Id = petId,
                CustomerId = customerId,
                Name = "Murphy",
                Type = "Dog"
            });

            dbContext.Providers.Add(new Provider
            {
                Id = providerId,
                Name = "Test Provider"
            });

            await dbContext.SaveChangesAsync();
        });

        var request = new
        {
            CustomerId = customerId,
            PetId = petId,
            ProviderId = providerId,
            StartTime =
                new DateTimeOffset(2026, 9, 10, 14, 0, 0, TimeSpan.Zero),

            EndTime = new DateTimeOffset(2026, 9, 10, 15, 0, 0, TimeSpan.Zero)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/bookings", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<CreateBookingResponse>();

        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.Id);
        Assert.Equal("Pending", body.Status);
    }

    [Fact]
    public async Task CreateBooking_WhenEndTimeIsBeforeStartTime_ReturnsBadRequest()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();

        var request = new
        {
            CustomerId = Guid.NewGuid(),
            PetId = Guid.NewGuid(),
            ProviderId = Guid.NewGuid(),

            StartTime = new DateTimeOffset(2026, 9, 10, 15, 0, 0, TimeSpan.Zero),

            EndTime = new DateTimeOffset(2026, 9, 10, 14, 0, 0, TimeSpan.Zero)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/bookings", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateBooking_WhenCustomerDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();

        var request = new
        {
            CustomerId = Guid.NewGuid(),
            PetId = Guid.NewGuid(),
            ProviderId = Guid.NewGuid(),

            StartTime = DateTimeOffset.UtcNow.AddDays(1),

            EndTime = DateTimeOffset.UtcNow.AddDays(1).AddHours(1)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/bookings", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task CreateBooking_WhenProviderHasOverlap_ReturnsConflict()
    {
        // Arrange
        await _factory.InitializeDatabaseAsync();

        var customerId = Guid.NewGuid();
        var petId = Guid.NewGuid();
        var providerId = Guid.NewGuid();

        var existingStart = new DateTimeOffset(2026, 9, 10, 14, 0, 0, TimeSpan.Zero);

        var existingEnd = new DateTimeOffset(2026, 9, 10, 15, 0, 0, TimeSpan.Zero);

        await _factory.ExecuteDbContextAsync(async dbContext =>
        {
            dbContext.Customers.Add(new Customer
            {
                Id = customerId,
                Name = "Test Customer",
                Email = $"{customerId}@example.com"
            });

            dbContext.Pets.Add(new Pet
            {
                Id = petId,
                CustomerId = customerId,
                Name = "Murphy",
                Type = "Dog"
            });

            dbContext.Providers.Add(new Provider
            {
                Id = providerId,
                Name = "Test Provider"
            });

            dbContext.Bookings.Add(new Booking
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                PetId = petId,
                ProviderId = providerId,
                StartTime = existingStart,
                EndTime = existingEnd
            });

            await dbContext.SaveChangesAsync();
        });

        var request = new
        {
            CustomerId = customerId,
            PetId = petId,
            ProviderId = providerId,

            StartTime = new DateTimeOffset(2026, 9, 10, 14, 30, 0, TimeSpan.Zero),

            EndTime = new DateTimeOffset(2026, 9, 10, 15, 30, 0, TimeSpan.Zero)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/bookings", request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    private sealed record CreateBookingResponse(Guid Id, string Status);
}