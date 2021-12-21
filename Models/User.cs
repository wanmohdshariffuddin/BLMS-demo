using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models
{
    public class User
    {
        [Key]
        public int USER_ID { get; set; }
        public string STAFF_NAME { get; set; }
        public string STAFF_NO { get; set; }
        public string STAFF_EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string ROLE { get; set; }
        public string ACCESS_LEVEL { get; set; }
        public string WRITE_ACCESS { get; set; }
    }
}
