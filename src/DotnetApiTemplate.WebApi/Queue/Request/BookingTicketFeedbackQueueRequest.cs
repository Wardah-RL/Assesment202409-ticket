using DotnetApiTemplate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.WebApi.Queue.Request
{
  public class BookingTicketFeedbackQueueRequest
  { 
    public Guid IdBookingTicketBroker { get; set; }
    public BookingOrderStatus Status { get; set; }
    public string? Note { get; set; }
    public Guid OrderCode { get; set; }
  }
}
