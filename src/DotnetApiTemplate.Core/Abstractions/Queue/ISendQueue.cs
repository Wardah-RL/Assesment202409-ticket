using DotnetApiTemplate.Core.Models.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Abstractions.Queue
{
    public interface ISendQueue
    {
        void Execute(SendQueueRequest paramQueue);
    }
}
