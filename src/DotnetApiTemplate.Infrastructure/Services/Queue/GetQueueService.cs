﻿using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions.Queue;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Models;
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
  public class GetQueueService : BackgroundService
  {
    private readonly ILogger<GetQueueService> _logger;
    private readonly IGetQueue _getQueue;
    private readonly IServiceProvider _serviceProvider;

    public GetQueueService(ILogger<GetQueueService> logger, IGetQueue getQueue, IServiceProvider serviceProvider)
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
        }
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}
