using Escc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Escc.EastSussexGovUK.Umbraco.Tests
{
    class FakeEmailSender : IEmailSender
    {
        public void Send(MailMessage message)
        {
            
        }

        public Task SendAsync(MailMessage message)
        {
            return null;
        }
    }
}
