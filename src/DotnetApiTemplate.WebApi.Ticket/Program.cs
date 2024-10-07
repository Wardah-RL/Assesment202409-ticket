using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Infrastructure;
using DotnetApiTemplate.Infrastructure.Services;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Infrastructure.Api;
using DotnetApiTemplate.Shared.Infrastructure.Cache;
using DotnetApiTemplate.Shared.Infrastructure.Contexts;
using DotnetApiTemplate.Shared.Infrastructure.Logging;
using DotnetApiTemplate.Shared.Infrastructure.Serialization.SystemTextJson;
using DotnetApiTemplate.WebApi.Ticket.HealthChecks;
using DotnetApiTemplate.WebApi.Ticket.Common;
using DotnetApiTemplate.WebApi.Ticket.Queue;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using System.Diagnostics;
using System.Globalization;
using System.Management;

var startTime = Stopwatch.GetTimestamp();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();
builder.Host.UseLogging();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthManager, AuthManager>();
builder.Services.AddSingleton<ISendQueue, SendQueueService>();
builder.Services.AddSingleton<IReciverQueue, EventQueueService>();
builder.Services.AddSingleton<IReciverQueue, BookingTicketQueueService>();
builder.Services.AddSingleton<IReciverQueue, PaymetQueueService>();
builder.Services.AddSingleton<IReciverQueue, PaymentNotificationService>();
builder.Services.AddSwaggerGen2();
builder.Services.AddAuth();
builder.Services.AddGlobalExceptionHandler();
builder.Services.AddCustomApiBehavior();
builder.Services.AddHostedService<ReciverQueueBackgroudService>();
builder.Services.AddTransient<EventQueueService>();
builder.Services.AddTransient<BookingTicketQueueService>();
builder.Services.AddTransient<PaymetQueueService>();
builder.Services.AddTransient<PaymentNotificationService>();

//builder.Services.AddRedisCache(builder.Configuration);
builder.Services.AddInternalMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers(options =>
    {
      options.Conventions.Add(
          new CustomRouteToken(
              "namespace",
              c => c.ControllerType.Namespace?.Split('.').Last()
          ));
      options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    })
    .AddJsonOptions(options =>
    {
      //remove based from discussion
      //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
      options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks()
    .AddCheck<ApplicationHealthCheck>("application")
    .AddCheck<DatabaseHealthCheck>("database");

builder.Services.AddSingleton<LocalizationMiddleware>();

var app = builder.Build();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStarted.Register(() =>
{
  var totalMillis = Math.Round(Stopwatch.GetElapsedTime(startTime).TotalMilliseconds, 2);
  Console.WriteLine($"Application has started, and took : {totalMillis}");
});

if (!app.Environment.IsProduction())
  app.UseSwaggerGenAndReDoc();

var options = new RequestLocalizationOptions
{
  DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
};
app.UseRequestLocalization(options);
app.UseMiddleware<LocalizationMiddleware>();

app.UseExceptionHandler();
app.UseMiddleware<MethodNotAllowedMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
  ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("cors");
app.UseAuth();
app.UseContext();
app.UseLogging();
app.UseRouting();
app.MapHealthChecks("/health-check", new HealthCheckOptions
{
  Predicate = _ => true,
  AllowCachingResponses = true,
  ResponseWriter = HealthCheckResponseWriter.WriteResponse,
  ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});
app.UseAuthorization();
app.MapControllers();
//Security Header
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseHsts();
app.Run();