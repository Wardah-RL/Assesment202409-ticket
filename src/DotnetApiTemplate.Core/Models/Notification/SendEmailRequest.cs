using SendGrid.Helpers.Mail;

namespace DotnetApiTemplate.Core.Models.Notification
{
    public class SendEmailRequest
    {
        public string Subject { get; set; } = string.Empty;
        public List<EmailAddress> Tos { get; set; } = new List<EmailAddress>();
        public string? HtmlContent { get; set; }
        public string? TextContent { get; set; }
        public bool IsHTML { get; set; }
    }
}
