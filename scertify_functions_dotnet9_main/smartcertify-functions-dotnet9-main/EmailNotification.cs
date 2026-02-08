using LSC.SmartCertify.Functions.Email;
using LSC.SmartCertify.Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LSC.SmartCertify.Functions
{
    public class EmailNotification
    {
        private readonly ILogger<EmailNotification> _logger;
        private readonly IEmailNotification emailNotification;

        public EmailNotification(ILogger<EmailNotification> logger, IEmailNotification emailNotification)
        {
            _logger = logger;
            this.emailNotification = emailNotification;
        }

        /*
         * Request payload
         {
            "userId":12,
            "notificationId":1
         }
         */
        [Function("EmailNotification")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function,"post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult("Invalid request body. Please provide a valid notification. Body cannot be empty");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            //we will parse our request body to this model
            UserEmailNotificationRequest? notification = JsonSerializer.Deserialize<UserEmailNotificationRequest>(requestBody, options);

            if (notification == null)
            {
                return new BadRequestObjectResult("Invalid request body. Please provide a valid notification details.");
            }

            //await emailNotification.SendEmailNotification(notification.UserId, notification.NotificationId);

            return new OkObjectResult("EmailNotification processed!");
        }
    }
}
