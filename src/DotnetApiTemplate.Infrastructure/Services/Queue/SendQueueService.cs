using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using DotnetApiTemplate.Shared.Abstractions.Models;
using Azure.Identity;
using DotnetApiTemplate.Core.Abstractions.Queue;
using DotnetApiTemplate.Core.Models.Queue;

namespace DotnetApiTemplate.Infrastructure.Services.Queue
{
    public class SendQueueService : ISendQueue
    {
        private readonly ConcurrentQueue<IDictionary<string, object>> _queue = new ConcurrentQueue<IDictionary<string, object>>();
        private readonly QueueConfiguration _queueConfiguration;

        public SendQueueService(QueueConfiguration queueConfiguration)
        {
            _queueConfiguration = queueConfiguration;
        }

        public async void Execute(SendQueueRequest paramQueue)
        {
            string connectionString = _queueConfiguration.Connection;
            QueueClient queue = new QueueClient(connectionString, paramQueue.QueueName);
            await queue.CreateIfNotExistsAsync();

            if (queue.Exists())
            {
                queue.Create();
                string jsonString = JsonConvert.SerializeObject(paramQueue);
                queue.SendMessage(jsonString);
            }
        }
    }
}
