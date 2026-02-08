using LSC.SmartCertify.Functions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LSC.SmartCertify.Functions.Email
{
    public interface IEmailNotification
    {
        Task SendEmailNotification(int userId, int notificationId);
        SmartCertifyContext GetDbContext();
    }

    public class EmailNotification : IEmailNotification
    {
        private readonly IConfiguration configuration;
        private readonly SmartCertifyContext dbContext;
        private readonly ILogger<EmailNotification> logger;

        public EmailNotification(IConfiguration configuration, 
            ILogger<EmailNotification> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }
        public SmartCertifyContext GetDbContext()
        {
            //Read connection string value from our local settings from project.
            string connectionString = configuration.GetConnectionString("DbContext");

            if (string.IsNullOrEmpty(connectionString))
            {
                logger.LogError("The connection string is null or empty.");
                throw new InvalidOperationException("The connection string has not been initialized.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<SmartCertifyContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var onlineCourseDbContext = new SmartCertifyContext(optionsBuilder.Options);
            return onlineCourseDbContext;
        }
        public async Task SendEmailNotification(int userId, int notificationId)
        {
            var onlineCourseDbContext = GetDbContext();

            var userProfile = await onlineCourseDbContext.UserProfiles.FindAsync(userId);
            var notification = await onlineCourseDbContext.Notifications.FindAsync(notificationId);
            var userNotification  = await onlineCourseDbContext.UserNotifications.Where(w=> w.UserId==userId && w.NotificationId == notificationId).FirstOrDefaultAsync();
            if(userProfile == null || notification == null)
            {
                logger.LogError($"User Profile or Notification not found for userId {userId} and notificationId {notificationId}");
                return;
            }

            if (userNotification!=null && userNotification.SentOn != null)
            {
                logger.LogInformation($"NotificationId {userNotification.NotificationId} for user {userId} has been sent already. Aborting email notification");
                return;
            }

            var apiKey = configuration["SENDGRID_API_KEY"];
            var from = new EmailAddress(configuration["From"]);
            var userFullName = $"{userProfile.LastName}, {userProfile.FirstName}";
            //var to = new EmailAddress(videoRequest.User.Email, userFullName);
            var to = new EmailAddress(userProfile.Email, userFullName);
            var cc = new EmailAddress(configuration["From"], "Learn Smart Coding");
            

            var sendGridMessage = new SendGridMessage()
            {
                From = from,
                Subject = userNotification?.EmailSubject
            };

            sendGridMessage.AddContent(MimeType.Html, GetEmailBody(userNotification, userFullName));
            sendGridMessage.AddTo(to);
            
            /*
            if (userProfile.Email != configuration["From"])
                sendGridMessage.AddCc(cc);
            */

            logger.LogInformation($"Sending email with payload: \n{sendGridMessage.Serialize()}");

            var response = await new SendGridClient(apiKey).SendEmailAsync(sendGridMessage).ConfigureAwait(false);
            string responseBody = await response.Body.ReadAsStringAsync();
            logger.LogInformation($"Status Code: {response.StatusCode}");
            logger.LogInformation($"Response Body: {responseBody}");

            if(response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                userNotification.NotificationSent = true;
                userNotification.SentOn = DateTime.UtcNow;
                await onlineCourseDbContext.SaveChangesAsync();
            }

            logger.LogInformation($"Response: {response.StatusCode}");
            Console.WriteLine(response.Headers);
        }

        private string GetEmailBody(UserNotification userNotification, string userFullName)
        {        
            var content =  userNotification.EmailContent.Replace("{UserName}", userFullName);
            return content;
        }
    }
}

