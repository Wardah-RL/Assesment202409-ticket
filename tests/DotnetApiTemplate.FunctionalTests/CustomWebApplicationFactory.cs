using DotnetApiTemplate.Persistence.Postgres;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace DotnetApiTemplate.FunctionalTests;

public class CustomWebApplicationFactory : WebApplicationFactory<ApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithDatabase("functionaltestdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(x =>
        {
            if (Output == null)
                return;

            x.ClearProviders();
            x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(Output));
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IDbContext>();
            services.RemoveAll<DbContextOptions<Persistence.Postgres.PostgresDbContext>>();
            services.RemoveAll<Persistence.Postgres.PostgresDbContext>();

            services.AddDbContext<Persistence.Postgres.PostgresDbContext>(x =>
                x.UseNpgsql(_dbContainer.GetConnectionString())
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddScoped((Func<IServiceProvider, IDbContext>)(serviceProvider => serviceProvider.GetRequiredService<Persistence.Postgres.PostgresDbContext>()));
        });
    }

    public ITestOutputHelper? Output { get; set; }

    public void SetOutPut(ITestOutputHelper output)
    {
        Output = output;
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}