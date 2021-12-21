using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.Admin
{
    public class AuditLog
    {
        public int ID { get; set; }

        public string Command { get; set; }

        [DisplayName("SP Name")]
        public string SPName { get; set; }

        [DisplayName("Screen")]
        public string ScreenPath { get; set; }

        [DisplayName("Old Value")]
        public string OldValue { get; set; }

        [DisplayName("New Value")]
        public string NewValue { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Date")]
        public string CreatedDt { get; set; }
    }
}
