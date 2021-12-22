using MimeKit;
using System.IO;
using System.Net.Mail;

namespace BLMS.Email
{
    public class LicenseBUEmail
    {
        #region EMAIL NOTIFICATION FOR APPROVED LICENSE (PIC)
        public void ApprovedLicensePIC(string LicenseName, string pathToFile)
        {
            string sendTo = "shuhaila.mutalib@edgenta.com";
            string sendBBC = "azhar.yusof@edgenta.com, nur.syahirah@edgenta.com";

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                LicenseName
                );

            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("noreply@blms.uemedgenta.com", "UEM Edgenta Berhad - Business License Management System");
                mm.To.Add(sendTo);
                mm.Bcc.Add(sendBBC);
                mm.Subject = "[APPROVED] BLMS - REQUEST NEW LICENSE HAS BEEN APPROVED AND WAITING FOR ADMINISTRATOR TO REGISTER.";
                mm.Body = messageBody;

                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Port = 25;
                    smtp.Host = "smtp2.edgenta.com";
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("user", "Password");
                    smtp.Send(mm);
                }
            }
        }
        #endregion

        #region EMAIL NOTIFICATION FOR APPROVED LICENSE (ADMIN)
        public void ApprovedLicenseAdmin(string LicenseName, string pathToFile)
        {
            string sendTo = "shuhaila.mutalib@edgenta.com";
            string sendBBC = "azhar.yusof@edgenta.com, nur.syahirah@edgenta.com";

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                LicenseName
                );

            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("noreply@blms.uemedgenta.com", "UEM Edgenta Berhad - Business License Management System");
                mm.To.Add(sendTo);
                mm.Bcc.Add(sendBBC);
                mm.Subject = "[APPROVED] BLMS - REQUEST NEW LICENSE HAS BEEN APPROVED AND WAITING FOR ADMINISTRATOR TO REGISTER.";
                mm.Body = messageBody;

                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Port = 25;
                    smtp.Host = "smtp2.edgenta.com";
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("user", "Password");
                    smtp.Send(mm);
                }
            }
        }
        #endregion

        #region EMAIL NOTIFICATION FOR REJECTED LICENSE (PIC)
        public void RejectedLicensePIC(string LicenseName, string pathToFile)
        {
            string sendTo = "shuhaila.mutalib@edgenta.com";
            string sendBBC = "azhar.yusof@edgenta.com, nur.syahirah@edgenta.com";

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                LicenseName
                );

            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("noreply@blms.uemedgenta.com", "UEM Edgenta Berhad - Business License Management System");
                mm.To.Add(sendTo);
                mm.Bcc.Add(sendBBC);
                mm.Subject = "[REJECTED] BLMS - REQUEST NEW LICENSE HAS BEEN REJECTED";
                mm.Body = messageBody;

                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Port = 25;
                    smtp.Host = "smtp2.edgenta.com";
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("user", "Password");
                    smtp.Send(mm);
                }
            }
        }
        #endregion
    }
}
