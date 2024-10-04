namespace DotnetApiTemplate.WebApi.Endpoints.Event.Request
{
  public class UpdateEventRequest : CreateEventRequest
  {
    public Guid IdEvent { get; set; }
  }
}
