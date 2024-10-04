using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using DotnetApiTemplate.Core.Abstractions.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using Microsoft.Extensions.Localization;
using DotnetApiTemplate.Shared.Abstractions.Models;

namespace DotnetApiTemplate.Infrastructure.Services.Queue
{
  public class ReciverBaseQueueService : IGetQueue
  {
    private readonly QueueConfiguration _queueConfiguration;

    public ReciverBaseQueueService(QueueConfiguration queueConfiguration)
    {
      _queueConfiguration = queueConfiguration;
    }
    public void Execute(string queueName)
    {
      QueueServiceClient serviceClient = new QueueServiceClient(_queueConfiguration.Connection);

      //list all queues in the storage account
      var getQueues = serviceClient.GetQueues().AsPages();

      //then you can write code to list all the queue names          
      foreach (Azure.Page<QueueItem> queuePage in getQueues)
      {
        foreach (QueueItem itemQueue in queuePage.Values)
        {
          Console.WriteLine(itemQueue.Name);
        }
      }
    }

    public virtual void cobax()
    {




    }
  }
}
