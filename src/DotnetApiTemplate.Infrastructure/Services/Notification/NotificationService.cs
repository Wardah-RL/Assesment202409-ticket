using Azure.Core;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Core.Models.Queue;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetApiTemplate.Core.Models.Notification;

namespace DotnetApiTemplate.Infrastructure.Services.Notification
{
    public abstract class NotificationService : IReciverQueue
    {
        public async Task Execute(string queueName)
        {

            //var sendEmail = SendEmailNotification();
            //SendEmail(sendEmail);

            var sendEmailRequest = await SendEmailNotification();
            await SendEmail(sendEmailRequest);
        }

        protected abstract Task<SendEmailRequest> SendPushNotification();
        protected abstract Task<SendEmailRequest> SendEmailNotification();


        //public virtual Task<SendEmailRequest> SendEmailNotification()
        //{
        //  var sendEmailRequest = new SendEmailRequest();
        //  return sendEmailRequest;
        //}

        public async Task SendEmail(SendEmailRequest request)
        {
            string plainTextContent = default;
            string htmlContent = default;

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("wardah@radyalabs.id", "Ticket Office");

            if (request.IsHTML)
                htmlContent = request.Content;
            else
                plainTextContent = request.Content;

            foreach (var to in request.Tos)
            {
                var msg = MailHelper.CreateSingleEmail(from, to, request.Subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            }
        }
    }
}
