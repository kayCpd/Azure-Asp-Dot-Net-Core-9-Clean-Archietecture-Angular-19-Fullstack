using System;
using System.Text;
using System.Text.Json;
using LSC.SmartCertify.Functions.Email;
using LSC.SmartCertify.Functions.Entities;
using LSC.SmartCertify.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LSC.SmartCertify.Functions
{
    public class EmailNotificationTimer
    {
        private readonly SmartCertifyContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmailNotificationTimer> logger;
        private readonly string _emailNotificationUrl = string.Empty;

        public EmailNotificationTimer(HttpClient httpClient, 
            IConfiguration configuration, 
            IEmailNotification emailNotification,
            ILogger<EmailNotificationTimer> logger)
        {
            _dbContext = emailNotification.GetDbContext();
            _httpClient = httpClient;
            this.logger = logger;
            _emailNotificationUrl = configuration.GetValue<string>("EmailNotificationURL") ?? "";
        }

        [Function("EmailNotificationTimerFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            logger.LogInformation($"Email Notification Timer executed at: {DateTime.UtcNow}");

            try
            {
                // Fetch unprocessed notifications
                var notifications = await _dbContext.UserNotifications
                    .Where(n => !n.NotificationSent).AsNoTracking()
                    .ToListAsync();

                if (!notifications.Any())
                {
                    logger.LogInformation("No new notifications to process.");
                    return;
                }

                foreach (var notification in notifications)
                {
                    var requestPayload = new UserEmailNotificationRequest()
                    {
                        UserId = notification.UserId,
                        NotificationId = notification.NotificationId
                    };
                    

                    var jsonPayload = new StringContent(JsonSerializer.Serialize(requestPayload), Encoding.UTF8, "application/json");

                    // Call the HTTP trigger function
                    var response = await _httpClient.PostAsync(_emailNotificationUrl, jsonPayload);

                    if (response.IsSuccessStatusCode)
                    {
                        notification.SentOn = DateTime.Now;
                        notification.NotificationSent = true;// Mark as processed
                        logger.LogInformation($"Successfully processed notification {notification.NotificationId} for user {notification.UserId}");
                    }
                    else
                    {
                        logger.LogWarning($"Failed to send notification {notification} for user {notification.UserId}. Status: {response.StatusCode}");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Error processing notifications: {ex.Message}");
            }
        }
    }
}
