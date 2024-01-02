using MailKit.Net.Smtp;
using MimeKit;

namespace MysteriousEncyclopedia.Models
{
    public class MailService
    {
        public void SendMail(string subject, string message, string senderMail, string receiverMail)
        {
            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", senderMail);

            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", receiverMail);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = message;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            mimeMessage.Subject = subject;

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate(senderMail, "ksijlvcwayfmnnyj");
            client.Send(mimeMessage);
            client.Disconnect(true);
        }
    }
}
