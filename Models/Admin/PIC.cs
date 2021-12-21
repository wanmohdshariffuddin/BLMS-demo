using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.Admin
{
    public class PIC
    {
        [Key]
        public int PICID { get; set; }

        public string OldPICStaffNo { get; set; }

        public string OldShortName { get; set; }

        public int OldUserTypeID { get; set; }

        [DisplayName("Staff No")]
        public string PICStaffNo { get; set; }

        [DisplayName("Staff Name")]
        public string PICName { get; set; }

        [DisplayName("Email")]
        public string PICEmail { get; set; }

        [DisplayName("Short Name")]
        public string ShortName { get; set; }

        public int UserTypeID { get; set; }

        [DisplayName("User")]
        public string UserType { get; set; }

        //check linked data
        public int ExistData { get; set; }

        public int ExistShortName { get; set; }

        public string UserName { get; set; }
    }
}
