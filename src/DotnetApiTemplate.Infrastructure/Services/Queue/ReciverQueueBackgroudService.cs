﻿using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Models;
using DotnetApiTemplate.WebApi.Endpoints.Payment.Notification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.Services.Queue
{
    public class ReciverQueueBackgroudService : BackgroundService
  {
    private readonly ILogger<ReciverQueueBackgroudService> _logger;
    private readonly IReciverQueue _getQueue;
    private readonly IServiceProvider _serviceProvider;

    public ReciverQueueBackgroudService(ILogger<ReciverQueueBackgroudService> logger, IReciverQueue getQueue, IServiceProvider serviceProvider)
    {
      _logger = logger;
      _getQueue = getQueue;
      _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        using (var scope = _serviceProvider.CreateScope())
        {
          scope.ServiceProvider.GetRequiredService<EventQueueService>().Execute("event");
          scope.ServiceProvider.GetRequiredService<BookingTicketQueueService>().Execute("bookingticket");
          scope.ServiceProvider.GetRequiredService<BookingFeedbackQueueService>().Execute("bookingfeedback");
          scope.ServiceProvider.GetRequiredService<PaymetQueueService>().Execute("payment");
          scope.ServiceProvider.GetRequiredService<PaymentSuccessService>().Execute("notification");
        }
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}
