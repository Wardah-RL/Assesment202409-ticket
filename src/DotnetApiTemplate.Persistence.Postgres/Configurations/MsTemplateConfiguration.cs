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
  public class MsTemplateConfiguration : BaseEntityConfiguration<MsTemplate>
  {
    protected override void EntityConfiguration(EntityTypeBuilder<MsTemplate> builder)
    {
      builder.Property(e => e.Code).HasMaxLength(100);
      builder.Property(e => e.Subject).HasMaxLength(100);
      builder.Property(e => e.TextContent).HasMaxLength(512);
    }
  }
}
