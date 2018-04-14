using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MailHelper
    {
        public void SenMail(string toEmailAddress, string subject, string content)
        {
            var fromEmailAddress = ConfigurationManager.ConnectionStrings["FromEmailAddress"].ToString();
            fromEmailAddress = "ditimniemvui2017@gmail.com";
            var fromEmailDisplayName = ConfigurationManager.ConnectionStrings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.ConnectionStrings["FromEmailPassword"].ToString();
            fromEmailPassword = "niemvui2017";
            var smtpHost = ConfigurationManager.ConnectionStrings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.ConnectionStrings["SMTPPort"].ToString();

            bool enabledSsl = bool.Parse(ConfigurationManager.ConnectionStrings["EnabledSSL"].ToString());

            string body = content;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            //client.UseDefaultCredentials = false
            client.EnableSsl = enabledSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);                            
        }
    }
}
