using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.Admin
{
    public class CertBody
    {
        [Key]
        public int CertBodyID { get; set; }

        [DisplayName("Certificate Body")]
        public string CertBodyName { get; set; }

        public int ExistData { get; set; }

        public string OldCertBodyName { get; set; }
    }
}
