using DotnetApiTemplate.WebApi.Endpoints.BookingTicket.Request;
using DotnetApiTemplate.WebApi.Endpoints.Payment.Request;
using FluentValidation;

namespace DotnetApiTemplate.WebApi.Endpoints.Payment.Validator
{
  public class CreatePaymentValidator : AbstractValidator<CreatePaymentRequest>
  {
    public CreatePaymentValidator()
    {
      RuleFor(e => e.TotalPayment).NotNull().NotEmpty();
      RuleFor(e => e.NamaPengirim).NotNull().NotEmpty();
      RuleFor(e => e.NoRekening).NotNull().NotEmpty();
    }
  }
}
