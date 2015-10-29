using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using SendGrid;

namespace Sogeti.Provisioning.WebJob.Helpers
{
    public class NotificationMail
    {
        private readonly string _filePath;

        public NotificationMail()
        {
            _filePath = "https://spsogeti.blob.core.windows.net/siteprovisioningfiles/Mail/AlertNotificationTemplate.html";
            ;
        }
        public async  void SendMailNotification(string sharePointAppUrl, string name, string useremail = null)
        {
                string description = $"Sharepoint site '{name}' created.";
                var urlValue = "<a href=" + name + ">" + sharePointAppUrl + "</a>";

                object[] parameters = { description , urlValue };
                var msgBody = GetMailMessage(parameters);

                var mailMessage = new SendGridMessage();

                var mailFrom = WebConfigurationManager.AppSettings["MailFrom"];
                var maintenanceEmailAddress = WebConfigurationManager.AppSettings["MaintenanceEmailAddress"];
                var mailUserName = WebConfigurationManager.AppSettings["MailUserID"];
                var mailPassword = WebConfigurationManager.AppSettings["MailPassword"];

                mailMessage.From = new MailAddress(mailFrom);
                var recipients = new List<string> {maintenanceEmailAddress};

                if (useremail != null)
                {
                    recipients.Add(useremail);
                }

                recipients.Add(mailFrom);
                mailMessage.AddTo(recipients);

                mailMessage.Subject =  description; 
                mailMessage.Html = msgBody;

                var credentials = new NetworkCredential(mailUserName, mailPassword);
                var transportWeb = new Web(credentials);
                await transportWeb.DeliverAsync(mailMessage);
            

        }

        private  string GetMailMessage(object[] parameters)
        {
            var rawEmail = LoadTemplate(_filePath);

            var urlValue = "<a href=" + parameters[0] + ">" + parameters[1] + "</a>";
            rawEmail = rawEmail.Replace("APPURL", urlValue);

            var mailBody = string.Format(rawEmail, parameters);

            return mailBody;

        }

        private  string LoadTemplate(string templatePath)
        {
            HtmlString html;
            using (var sr = new StreamReader("Resources/AlertNotificationTemplate.html"))
            {
                var file = sr.ReadToEnd();
                html = new HtmlString(file);
            }
           

            string rawEmail = html.ToString();
            return rawEmail;
        }

    }
}
