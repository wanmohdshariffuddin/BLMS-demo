using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.License
{
    public class LicenseAllUser
    {
        [Key]
        public int LicenseID { get; set; }

        [DisplayName("Type")]
        public string CategoryName { get; set; }

        [DisplayName("License Name")]
        public string LicenseName { get; set; }

        [DisplayName("Registration No")]
        public string RegistrationNo { get; set; }

        [DisplayName("Business Div")]
        public string DivName { get; set; }

        [DisplayName("Unit")]
        public string UnitName { get; set; }

        public string IssuedDT { get; set; }

        [DisplayName("Expired Date")]
        public string ExpiredDT { get; set; }

        public string PIC1Name { get; set; }

        public string PIC2Name { get; set; }

        public string PIC3Name { get; set; }

        public string UserType { get; set; }

        //Flag
        public bool isRequested { get; set; }

        public bool isApproved { get; set; }

        public bool isRejected { get; set; }

        public bool isRegistered { get; set; }

        public bool isRenewed { get; set; }

        //Renew Remainder
        public DateTime RenewReminderDT { get; set; }

        //License File
        [Key]
        public int LicenseFileId { get; set; }

        public string LicenseFileName { get; set; }

        //check license had file
        public bool hasFile { get; set; }
    }
}
