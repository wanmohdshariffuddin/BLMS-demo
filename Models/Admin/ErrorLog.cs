using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.Admin
{
    public class ErrorLog
    {
        public int ID { get; set; }

        [DisplayName("Module")]
        public string PageName { get; set; }

        [DisplayName("Error Message")]
        public string ErrorMessage { get; set; }

        public string Method { get; set; }

        [DisplayName("Line No.")]
        public string LineNumber { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Date")]
        public string CreatedDt { get; set; }
    }
}
