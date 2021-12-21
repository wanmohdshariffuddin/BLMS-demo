using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.Admin
{
    public class BusinessDiv
    {
        #region OLD VALUE
        public string OldDivName { get; set; }
        #endregion

        #region PRIMARY KEY
        [Key]
        public int DivID { get; set; }
        #endregion

        #region CURRENT VALUE
        [DisplayName("Business Division")]
        public string DivName { get; set; }
        #endregion

        #region CHECK
        //CHECK DUPLICATE
        public int ExistData { get; set; }

        //CHECK LINKED DATA
        public int LinkedData { get; set; }
        #endregion

        public string UserName { get; set; }
    }
}
