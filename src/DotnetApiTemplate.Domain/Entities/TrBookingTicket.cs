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
  public class TrBookingTicket : BaseEntity
  {
    public Guid IdBookingTicketBroker { get; set; }
    [ForeignKey(nameof(Event))]
    public Guid IdEvent { get; set; }
    public MsEvent? Event { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; } 
    public string? phone { get; set; }
    public int CountTicket { get; set; }
    public BookingOrderStatus Status { get; set; }
    public DateTime DateEvent { get; set; }
  }
}
