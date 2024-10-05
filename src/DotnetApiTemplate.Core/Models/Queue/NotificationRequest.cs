using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Models.Queue
{
  public class NotificationRequest
  {
    public Guid Id { get; set; }
    public string Scenario { get; set; } = string.Empty;
  }
}
