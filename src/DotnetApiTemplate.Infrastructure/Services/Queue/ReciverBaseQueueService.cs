using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using Microsoft.Extensions.Localization;
using DotnetApiTemplate.Shared.Abstractions.Models;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Threading;
using DotnetApiTemplate.Core.Abstractions;

namespace DotnetApiTemplate.Infrastructure.Services.Queue
{
    public class ReciverBaseQueueService : IReciverQueue
  {
    private readonly QueueConfiguration _queueConfiguration;
    private readonly IServiceProvider _serviceProvider;
    private readonly CancellationToken cancellationToken;
    public ReciverBaseQueueService(QueueConfiguration queueConfiguration, IServiceProvider serviceProvider)
    {
      _queueConfiguration = queueConfiguration;
      _serviceProvider = serviceProvider;
    }
    public async Task Execute(string queueName)
    {
      string connectionString = _queueConfiguration.Connection;

      QueueClient queue = new QueueClient(connectionString, queueName);

      if (queue.Exists())
      {
        try
        {
          var listReceiveMessages = queue.ReceiveMessages(maxMessages: 32).Value.ToList();

          foreach (var message in listReceiveMessages)
          {
            var getMessage = JsonConvert.DeserializeObject<SendQueueRequest>(message.Body.ToString());
            if (getMessage == null)
              continue;

            if (getMessage.Message is null || getMessage.Scenario is null)
              continue;

            bool isDeleteQueue = false;
            using (var scope = _serviceProvider.CreateScope())
            {
              var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();
              isDeleteQueue = await QueueContent(dbContext,cancellationToken, getMessage.Scenario,  getMessage);
            }

            //remove queue
            if (isDeleteQueue)
              queue.DeleteMessage(message.MessageId, message.PopReceipt);
          }
        }
        catch (Exception ex)
        {

        }
      }
    }

    public async virtual Task<bool> QueueContent(IDbContext dbContext, CancellationToken cancellationToken, string scenario, SendQueueRequest message) 
    {
      return true;
    }
  }
}
