using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.Admin
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public string OldCategoryName { get; set; }

        public string OldDesc { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("License Type")]
        public string CategoryName { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [DisplayName("Description")]
        public string Description { get; set; }

        //check duplicate
        public int ExistData { get; set; }

        public string UserName { get; set; }
    }
}
