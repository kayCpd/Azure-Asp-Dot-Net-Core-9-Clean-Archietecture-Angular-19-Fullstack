using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSC.SmartCertify.Functions.Models
{
    public class UserEmailNotificationRequest
    {
        public int UserId { get; set; }
        public int NotificationId { get; set; }
    }
}
