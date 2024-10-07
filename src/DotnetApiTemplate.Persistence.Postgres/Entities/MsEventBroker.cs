using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Persistence.Postgres.Entities
{
  public class MsEventBroker : BaseEntity
  {
    public string Name { get; set; } = string.Empty!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CountTicket { get; set; }
    public string Location { get; set; } = string.Empty;
    public int Price { get; set; }
    public ICollection<TrBookingTicketBroker> BookingBroker { get; set; } = new HashSet<TrBookingTicketBroker>();
  }
}
