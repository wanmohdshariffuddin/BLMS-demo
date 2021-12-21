using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.License
{
    public class LicenseHQ
    {
        [DisplayName("#")]
        public int indexNo { get; set; }

        [Key]
        public int LicenseID { get; set; }

        //Category
        public int CategoryID { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Type")]
        public string CategoryName { get; set; }

        //License Details
        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("License Name")]
        public string LicenseName { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Registration No")]
        public string RegistrationNo { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Serial No")]
        public string SerialNo { get; set; }

        //Business Div
        public int DivID { get; set; }


        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Business Div")]
        public string DivName { get; set; }

        //Business Unit
        public int UnitID { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Business Unit")]
        public string UnitName { get; set; }

        [DisplayName("Issued Date")]
        public string IssuedDT { get; set; }

        [DisplayName("Expired Date")]
        public string ExpiredDT { get; set; }

        //PIC 1
        [Column(TypeName = "nvarchar(150)")]
        public string PIC1StaffNo { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("PIC 1 Name")]
        public string PIC1Name { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("PIC 1 Email")]
        public string PIC1Email { get; set; }

        //PIC 2
        [Column(TypeName = "nvarchar(150)")]
        public string PIC2StaffNo { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("PIC 2 Name (Optional)")]
        public string PIC2Name { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("PIC 2 Email")]
        public string PIC2Email { get; set; }

        //PIC 3
        [Column(TypeName = "nvarchar(150)")]
        public string PIC3StaffNo { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("PIC 3 Name (Optional)")]
        public string PIC3Name { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("PIC 3 Email")]
        public string PIC3Email { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Remarks")]
        public string Remarks { get; set; }

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

        //check linked data
        public int ExistData { get; set; }

        //check license had file
        public bool hasFile { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("Rejection Remarks")]
        public string RejectionRemarks { get; set; }

        //History
        public int HistoryLicenseID { get; set; }

        public string HistoryLicenseName { get; set; }

        public string HistoryRegistrationNo { get; set; }

        public string HistorySerialNo { get; set; }

        public string HistoryPIC1Name { get; set; }

        public string HistoryPIC2Name { get; set; }

        public string HistoryPIC3Name { get; set; }

        //Flag
        public bool HistoryisRequested { get; set; }

        public bool HistoryisApproved { get; set; }

        public bool HistoryisRejected { get; set; }

        public bool HistoryisRegistered { get; set; }

        public bool HistoryisRenewed { get; set; }

        public bool HistoryhasFile { get; set; }

        public string HistoryIssuedDT { get; set; }

        public string HistoryExpiredDT { get; set; }

        public DateTime HistoryRenewReminderDT { get; set; }
    }
}
