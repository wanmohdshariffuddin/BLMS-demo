using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.SOP
{
    public class SOP
    {
        #region tblSOP
        [Key]
        public int SOPID { get; set; }

        [DisplayName("SOP Name")]
        public string SOPName { get; set; }

        [DisplayName("Ref No")]
        public string RefNo { get; set; }

        [DisplayName("Rev No")]
        public string RevNo { get; set; }

        public string EffectiveDT { get; set; }

        public bool HasFile { get; set; }
        #endregion

        #region tblSOPFileOnSystem
        public string SOPFileName { get; set; }
        #endregion

        #region OLD DATA
        public string OldSOPName { get; set; }

        public string OldSOPFileName { get; set; }
        #endregion

        public int ExistData { get; set; }

        public string UserName { get; set; }
    }
}
