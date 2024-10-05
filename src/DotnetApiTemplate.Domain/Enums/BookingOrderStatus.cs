using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Domain.Enums
{
  public enum BookingOrderStatus
  {
    Processed,
    Ordered,
    Payment,
    Done,
    failed,
  }
}
