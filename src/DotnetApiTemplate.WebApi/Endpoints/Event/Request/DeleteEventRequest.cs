using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.Event.Request
{
  public class DeleteEventRequest
  {
    [FromRoute(Name = "IdEvent")] public Guid IdEvent { get; set; }
  }
}
