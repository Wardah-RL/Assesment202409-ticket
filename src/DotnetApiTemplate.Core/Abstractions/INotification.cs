using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Abstractions
{
  public interface INotification
  {
    Task Execute(string queueName);
  }
}
