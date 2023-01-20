using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace ClassLibTeam09.Mail
{
    public class BoekersMail
    {
        public BoekersMail(string mailTo)
        {
            To = mailTo;
            From = MailServer.MailFrom;
        }
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

        public void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage(From, To);
                mail.Body = Body;
                mail.Subject = Subject;

                var smtp = MailServer.GetSmtpClient();
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }
            
        }
    }
}
