using System.Net;
using System.Net.Mail;
using Training.DAL.Entities;

namespace TrainingStudent.Helpers
{
    public static class EmailSettings
    {
        public  static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.sendgrid.net", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("apikey", "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");
            client.Send("tasneemahmedshehata@gmail.com", email.To, email.Title, email.Body);
        }
    }
}
