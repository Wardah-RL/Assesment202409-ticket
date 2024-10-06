using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Shared.Abstractions.Models
{
  public class SendGridConfiguration
  {
    public string Key { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmailName { get; set; } = string.Empty;
  }
}
