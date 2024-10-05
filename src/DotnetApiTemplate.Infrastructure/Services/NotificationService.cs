using DotnetApiTemplate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Infrastructure.Services
{
  public class NotificationService : INotification
  {
    public Task Execute(string queueName)
    {






      return Task.CompletedTask;
    }
  }
}
