using BLMS.Models.License;
using MimeKit;
using System.IO;
using System.Net.Mail;

namespace BLMS.Email
{
    public class LicenseHQEmail
    {
        #region EMAIL NOTIFICATION FOR REQUEST LICENSE
        public void RequestLicense(LicenseHQ licenseHQ, string UnitName, string CategoryName, string PIC1Email, string pathToFile)
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
                licenseHQ.LicenseName,
                CategoryName,
                licenseHQ.PIC1Name,
                PIC1Email
                );

            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("noreply@blms.uemedgenta.com", "UEM Edgenta Berhad - Business License Management System");
                mm.To.Add(sendTo);
                mm.Bcc.Add(sendBBC);
                mm.Subject = "[REQUEST LICENSE] BLMS - NEW REQUEST FOR LICENSE (HQ USER)";
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

         #region EMAIL NOTIFICATION FOR REQUEST RENEWAL
        public void RequestRenewal(LicenseHQ licenseHQ, string pathToFile)
        {
            string sendTo = "shuhaila.mutalib@edgenta.com";
            string sendBBC = "azhar.yusof@edgenta.com, nur.syahirah@edgenta.com";

            var builder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                licenseHQ.UnitName,
                licenseHQ.LicenseName,
                licenseHQ.CategoryName,
                licenseHQ.PIC1Name,
                licenseHQ.PIC1Email
                );

            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("noreply@blms.uemedgenta.com", "UEM Edgenta Berhad - Business License Management System");
                mm.To.Add(sendTo);
                mm.Bcc.Add(sendBBC);
                mm.Subject = "[REQUEST RENEWAL] BLMS - NEW REQUEST FOR LICENSE RENEWAL (HQ USER)";
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
