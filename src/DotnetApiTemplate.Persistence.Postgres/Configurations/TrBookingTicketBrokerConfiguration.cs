using DotnetApiTemplate.Persistence.Postgres.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations
{

  public class TrBookingTicketBrokerConfiguration : BaseEntityConfiguration<TrBookingTicketBroker>
  {
    protected override void EntityConfiguration(EntityTypeBuilder<TrBookingTicketBroker> builder)
    {
      builder.Property(e => e.Note).HasMaxLength(500);
    }
  }
}
