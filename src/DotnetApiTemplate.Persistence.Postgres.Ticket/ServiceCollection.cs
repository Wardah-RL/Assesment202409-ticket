using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Infrastructure.Initializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace DotnetApiTemplate.Persistence.Postgres.Ticket;

public static class ServiceCollection
{
    public const string DefaultConfigName = "DefaultConnection";

    public static void AddPostgresTicketDbContext(this IServiceCollection services, IConfiguration configuration,
        string connStringName = DefaultConfigName)
    {
        services.AddDbContext<TicketPostgresDbContext>(
            x => x.UseNpgsql(configuration.GetConnectionString(connStringName))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<TicketPostgresDbContext>());

        services.AddInitializer<AutoMigrationService>();
    }

    public static void AddPostgresDbContext(this IServiceCollection services, IConfiguration configuration,
        Action<NpgsqlDbContextOptionsBuilder>? action,
        string connStringName = DefaultConfigName)
    {
        services.AddDbContext<TicketPostgresDbContext>(x =>
            x.UseNpgsql(configuration.GetConnectionString(connStringName), action)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<TicketPostgresDbContext>());

        services.AddInitializer<AutoMigrationService>();
    }
}