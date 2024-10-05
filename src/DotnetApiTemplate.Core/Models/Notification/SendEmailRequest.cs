using SendGrid.Helpers.Mail;

namespace DotnetApiTemplate.Core.Models.Notification
{
    public class SendEmailRequest
    {
        public string Subject { get; set; } = string.Empty;
        public List<EmailAddress> Tos { get; set; } = new List<EmailAddress>();
        public string Content { get; set; } = string.Empty;
        public bool IsHTML { get; set; }
    }
}
