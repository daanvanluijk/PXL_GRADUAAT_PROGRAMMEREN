using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace ClassLibTeam09.Mail
{
    static class MailServer
    {
        const string mailServer = "smtp.gmail.com";
        const int portNumber = 587;
        const string userName = "BoekersHotels@gmail.com";
        const string password = "Boekers123.";
        public static string MailFrom => userName;
        public static SmtpClient GetSmtpClient()
        {
            var smtpClient = new SmtpClient(mailServer, portNumber);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                
                UserName = userName,
                Password = password
                
            };
            smtpClient.EnableSsl = true;
            return smtpClient;

        }
    }
}
