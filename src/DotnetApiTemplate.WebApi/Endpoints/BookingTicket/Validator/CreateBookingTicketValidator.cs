using DotnetApiTemplate.WebApi.Endpoints.BookingTicket.Request;
using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using FluentValidation;

namespace DotnetApiTemplate.WebApi.Endpoints.BookingTicket.Validator
{
    public class CreateBookingTicketValidator : AbstractValidator<CreateBookingTicketRequest>
  {
    public CreateBookingTicketValidator()
    {
      RuleFor(e => e.CountTicket).NotNull().NotEmpty();
    }
  }
}
