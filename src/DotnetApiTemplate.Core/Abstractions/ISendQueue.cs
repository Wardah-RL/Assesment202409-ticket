using DotnetApiTemplate.Core.Models.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Abstractions
{
    public interface ISendQueue
    {
        Task Execute(SendQueueRequest paramQueue);
    }
}
