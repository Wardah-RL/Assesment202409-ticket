using DotnetApiTemplate.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Domain.Entities
{
  public class MsTemplate : BaseEntity
  {
    public string Code { get; set; } = null!;
    public bool IsHtml { get; set; }
    public string Subject { get; set; } = null!;
    public string? HTMLContent { get; set; }
    public string? TextContent { get; set; }
  }
}
