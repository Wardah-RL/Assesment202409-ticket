namespace DotnetApiTemplate.WebApi.Ticket.Contracts.Requests;

public class SetCacheRequest
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}