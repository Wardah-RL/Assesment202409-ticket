using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.Event.Request
{
  public class DeleteEventRequest
  {
    [FromRoute(Name = "idEvent")] public Guid IdEvent { get; set; }
  }
}
