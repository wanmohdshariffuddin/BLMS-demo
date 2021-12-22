using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BLMS.Models.SOP
{
    [Table("tblCompetentPersonnel")]
    public partial class Competent
    {
        [StringLength(150)]
        [Display(Name = "Business Division")]
        public string BusinessDiv { get; set; }

        [StringLength(150)]
        [Display(Name = "Business Unit")]
        public string BusinessUnit { get; set; }

        [Key]
        [Column("PersonnelID")]
        [Display(Name = "Personnel ID")]
        public int PersonnelId { get; set; }

        [StringLength(500)]
        [Display(Name = "Name")]
        public string PersonnelName { get; set; }

        [Column("AppointedDT")]
        [Display(Name = "Appointment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? AppointedDt { get; set; }

        [Column("ICNo")]
        [StringLength(15)]
        [Display(Name = "IC No")]
        public string Icno { get; set; }

        [StringLength(100)]
        [Display(Name = "Certificate Body")]
        public string CertFrom { get; set; }

        [StringLength(100)]
        [Display(Name = "Certificate Type")]
        public string CertType { get; set; }

        [StringLength(100)]
        [Display(Name = "Certificate No")]
        public string CertNo { get; set; }

        [Column("ExpiredDT")]
        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ExpiredDt { get; set; }

        [Display(Name = "Year Awarded")]
        public int? YearAwarded { get; set; }

        [StringLength(200)]
        [Display(Name = "File")]
        public string CertFileName { get; set; }

        [StringLength(200)]
        [Display(Name = "Registration No")]
        public string RegNo { get; set; }

        [StringLength(200)]
        public string Branch { get; set; }

        [Column("CreatedDT")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime CreatedDt { get; set; }

        [Column("CreatedBY")]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column("UpdatedDT")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime UpdatedDt { get; set; }

        [Column("UpdatedBY")]
        [StringLength(50)]
        public string UpdatedBy { get; set; }


        public string AppointedDT { get; set; }
        public string ExpiredDT { get; set; }

        public string YearAward { get; set; }

        public string UserRole { get; set; }
    }
}
