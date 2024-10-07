using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Models.Queue
{
  public class BookingTicketQueueRequest
  {
    public Guid IdEvent { get; set; }
    public Guid IdBookingTicket { get; set; }
    public string Name {  get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int CountTicket { get; set; }
    public DateTime DateEvent { get; set; }
  }
}
