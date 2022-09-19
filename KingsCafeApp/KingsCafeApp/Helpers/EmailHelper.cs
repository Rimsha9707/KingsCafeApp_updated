using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace KingsCafeApp.Helpers
{
    public class EmailHelper
    {
        public string EmailSender(string Email, string Subject, string Message)
        {
            try
            {

                // EMAIL SENDING ================================================================
                MailMessage mail = new MailMessage();
                mail.To.Add(Email);
                mail.From = new MailAddress("webemailservice007@gmail.com", Subject, System.Text.Encoding.UTF8);
                mail.Subject = Subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.Body = Message;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new System.Net.NetworkCredential("webemailservice007@gmail.com", "djibwzyjkwvahnxl");
                client.EnableSsl = true;

                client.Send(mail);

                return "Email Sent...";
            }
            catch (Exception ex)
            {
                return "Something Went Wring \n\n" + ex.Message;
            }
        }
        public string MessageSender(string Phone, string Message)
        {
            try
            {
                // SMS SENDING ================================================================
                var uri = "https://lifetimesms.com/json?api_token=a976f26420e66b7ed08aac4034b32b7f68ec633295&api_secret=Testing&to=" + Phone + "&from=Sport&message=" + Message;

                WebRequest request = WebRequest.Create(uri);
                WebResponse response = request.GetResponse();

                return "Message Sent...";
            }
            catch (Exception ex)
            {
                return "Something Went Wring \n\n" + ex.Message;
            }
        }


    }
}