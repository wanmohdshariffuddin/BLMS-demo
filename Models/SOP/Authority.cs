using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.SOP
{
    public class Authority
    {
        [Key]
        public int AuthorityID { get; set; }

        [DisplayName("Authority Link")]
        public string AuthorityLink { get; set; }

        [DisplayName("Authority Name")]
        public string AuthorityName { get; set; }

        public string OldAuthorityLink { get; set; }

        public string OldAuthorityName { get; set; }

        public int ExistData { get; set; }

        public string UserName { get; set; }
    }
}
