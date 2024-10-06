using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Models.Queue
{
    public class SendQueueRequest
    {
        public string Message { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string Scenario { get; set; } = string.Empty;
    }
}
