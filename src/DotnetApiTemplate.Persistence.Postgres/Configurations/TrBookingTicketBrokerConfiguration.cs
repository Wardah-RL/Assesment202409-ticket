using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations
{

  public class TrBookingTicketBrokerConfiguration : BaseEntityConfiguration<TrBookingTicket>
  {
    protected override void EntityConfiguration(EntityTypeBuilder<TrBookingTicket> builder)
    {
      builder.Property(e => e.Note).HasMaxLength(500);
    }
  }
}
