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
using DotnetApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Twilio.Http;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Queues;
using Newtonsoft.Json;
using DotnetApiTemplate.Shared.Abstractions.Models;
using System.Net;

namespace DotnetApiTemplate.Infrastructure.Ticket.Services
{
  public abstract class NotificationService : IReciverQueue
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly CancellationToken cancellationToken;
    private readonly QueueConfiguration _queueConfiguration;
    private readonly SendGridConfiguration _sendGridConfiguration;

    public NotificationService(IServiceProvider serviceProvider, QueueConfiguration queueConfiguration, SendGridConfiguration sendGridConfiguration)
    {
      _serviceProvider = serviceProvider;
      _queueConfiguration = queueConfiguration;
      _sendGridConfiguration = sendGridConfiguration;
    }
    public async Task Execute(string queueName)
    {
      string connectionString = _queueConfiguration.Connection;

      QueueClient queue = new QueueClient(connectionString, queueName);

      if (queue.Exists())
      {
        try
        {
          var listReceiveMessages = queue.ReceiveMessages(maxMessages: 32).Value.ToList();

          foreach (var message in listReceiveMessages)
          {
            var getMessage = JsonConvert.DeserializeObject<SendQueueRequest>(message.Body.ToString());
            if (getMessage == null)
              continue;

            if (getMessage.Message is null || getMessage.Scenario is null)
              continue;

            bool isDeleteQueue = false;

            using (var scope = _serviceProvider.CreateScope())
            {
              var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();
              var getNotification = JsonConvert.DeserializeObject<NotificationRequest>(getMessage.Message);

              var template = await Template(dbContext, getNotification.Scenario);
              await Prepare(dbContext, cancellationToken, getNotification);
              var sendEmailRequest = await SendEmailNotification(dbContext, cancellationToken, template);
              var IsStatusSend = await SendEmail(sendEmailRequest);

              if (IsStatusSend)
                isDeleteQueue = true;
            }

            //remove queue
            if (isDeleteQueue)
              queue.DeleteMessage(message.MessageId, message.PopReceipt);
          }
        }
        catch (Exception ex)
        {

        }
      }
    }

    protected abstract Task Prepare(IDbContext dbContext, CancellationToken cancellationToken, NotificationRequest request);

    protected abstract Task<SendEmailRequest> SendPushNotification();

    protected abstract Task<SendEmailRequest> SendEmailNotification(IDbContext dbContext, CancellationToken cancellationToken, SendEmailRequest request);

    public async Task<bool> SendEmail(SendEmailRequest request)
    {
      string plainTextContent = default;
      string htmlContent = default;

      var apiKey = _sendGridConfiguration.Key;
      var client = new SendGridClient(apiKey);
      var from = new EmailAddress(_sendGridConfiguration.Email, _sendGridConfiguration.EmailName);

      if (request.IsHTML)
        htmlContent = request.HtmlContent;
      else
        plainTextContent = request.TextContent;

      var isStatusSend = false;
      foreach (var to in request.Tos)
      {
        var msg = MailHelper.CreateSingleEmail(from, to, request.Subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
          isStatusSend = true;
      }

      return isStatusSend;
    }

    public async Task<SendEmailRequest> Template(IDbContext dbContext, string idScenario)
    {
      var getTemplate = await dbContext.Set<MsTemplate>()
                              .Where(e => e.Code == idScenario)
                              .FirstOrDefaultAsync(cancellationToken);

      if (getTemplate == null)
        throw new Exception("Template is not found");

      SendEmailRequest template = new SendEmailRequest
      {
        HtmlContent = getTemplate.HTMLContent,
        TextContent = getTemplate.TextContent,
        Subject = getTemplate.Subject,
        IsHTML = getTemplate.IsHtml,
      };
      return template;
    }
  }
}
