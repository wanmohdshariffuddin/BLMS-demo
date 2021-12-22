using MimeKit;
using System.IO;
using System.Net.Mail;

namespace BLMS.Email
{
    public class LicenseSiteEmail
    {
        #region EMAIL NOTIFICATION FOR REGISTER LICENSE
        public void RegisterLicense(string LicenseName, string UnitName, string CategoryName, string PIC1Name, string PIC1Email, string pathToFile)
        {
            string sendTo = "shuhaila.mutalib@edgenta.com";
            string sendBBC = "azhar.yusof@edgenta.com, nur.syahirah@edgenta.com";

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                UnitName,
                LicenseName,
                CategoryName,
                PIC1Name,
                PIC1Email
                );

            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("noreply@blms.uemedgenta.com", "UEM Edgenta Berhad - Business License Management System");
                mm.To.Add(sendTo);
                mm.Bcc.Add(sendBBC);
                mm.Subject = "[REGISTER] BLMS - NEW REGISTRATION LICENSE FOR SITE USER";
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

        #region EMAIL NOTIFICATION FOR RENEWAL LICENSE
        public void RenewalLicense(string LicenseName, string UnitName, string CategoryName, string PIC1Name, string PIC1Email, string pathToFile)
        {
            string sendTo = "shuhaila.mutalib@edgenta.com";
            string sendBBC = "azhar.yusof@edgenta.com, nur.syahirah@edgenta.com";

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                UnitName,
                LicenseName,
                CategoryName,
                PIC1Name,
                PIC1Email
                );

            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("noreply@blms.uemedgenta.com", "UEM Edgenta Berhad - Business License Management System");
                mm.To.Add(sendTo);
                mm.Bcc.Add(sendBBC);
                mm.Subject = "[RENEWAL] BLMS - NEW RENEWAL LICENSE FOR SITE USER";
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
