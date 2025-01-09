using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.Constants
{
    public static class Constants
    {
        public static int BAD_REQUEST_STATUS_CODE = 400;
        public static int OK_STATUS_CODE = 200;
        public static int INTERNAL_SERVER_ERROR = 500;
        public static int NOT_FOUND = 404;
        public static string EMAIL_OTP_QUEUE = "otp.queue";
        public static string MESSAGE_BUS_EMAIL_EXCHANGE_NAME = "email.exchange";
        
        public static string ORGANIZATION_TYPE_LOOKUP = "OrganizationType";
        
        public static List<string> TICKET_TYPE_LOOKUP = new List<string> { "Free", "Paid" };
    }
}
