using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Domain.Entities
{
  public class TrBookingTicketBroker : BaseEntity
  {
    [ForeignKey(nameof(EventBroker))]
    public Guid IdEventBroker { get; set; }
    public MsEventBroker? EventBroker { get; set; }

    [ForeignKey(nameof(User))]
    public Guid IdUser { get; set; }
    public User? User { get; set; }

    public int CountTicket { get; set; }
    public BookingOrderStatus Status { get; set; }
  }
}
