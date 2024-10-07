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
    [ForeignKey(nameof(Event))]
    public Guid IdEvent { get; set; }
    public MsEvent? Event { get; set; }

    [ForeignKey(nameof(User))]
    public Guid IdUser { get; set; }
    public User? User { get; set; }

    public int CountTicket { get; set; }
    public BookingOrderStatus Status { get; set; }
    public string? Note { get; set; }
    public Guid? OrderCode {  get; set; }
    public DateTime DateEvent {  get; set; }
    public ICollection<TrPayment> Payment { get; set; } = new HashSet<TrPayment>();
  }
}
