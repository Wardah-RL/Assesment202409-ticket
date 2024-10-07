using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Domain.Entities
{
  public class TrPayment : BaseEntity
  {
    [ForeignKey(nameof(BookingTicket))]
    public Guid IdBookingTicket { get; set; }
    public TrBookingTicket? BookingTicket { get; set; }

    [ForeignKey(nameof(Bank))]
    public Guid IdBank { get; set; }
    public MsBank? Bank { get; set; }
    public int TotalPayment { get; set; }
    public string NamaPembayar { get; set; } = string.Empty;
    public string NoRekening { get; set; } = string.Empty;
  }
}
