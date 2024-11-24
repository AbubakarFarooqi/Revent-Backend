using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Services.IServices
{
    public interface IEmailService
    {
        void SendMail(string subject, string body,string to );
    }
}
