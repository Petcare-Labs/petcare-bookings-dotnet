using Testcontainers.PostgreSql;

namespace PetCare.Bookings.IntegrationTests.Infrastructure;

public sealed class PostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder()
            .WithImage("postgres:18-alpine")
            .WithDatabase("petcare_bookings_tests")
            .WithUsername("petcare")
            .WithPassword("petcare_test")
            .Build();

    public string ConnectionString =>
        _container.GetConnectionString();

    public Task InitializeAsync()
    {
        return _container.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _container.DisposeAsync().AsTask();
    }
}
