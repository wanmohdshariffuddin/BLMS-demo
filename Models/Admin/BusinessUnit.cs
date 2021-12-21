using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLMS.Models.Admin
{
    public class BusinessUnit
    {
        #region OLD VALUE
        public int OldDivID { get; set; }
        public string OldUnitName { get; set; }
        public string OldHoCName { get; set; }
        public string OldHoCEmail { get; set; }
        #endregion

        #region PRIMARY KEY
        [Key]
        public int UnitID { get; set; }
        #endregion

        #region CURRENT VALUE
        public int DivID { get; set; }

        [DisplayName("Business Division")]
        public string DivName { get; set; }

        [DisplayName("Business Unit")]
        public string UnitName { get; set; }

        [DisplayName("Head of Company")]
        public string HoCName { get; set; }

        public string HoCEmail { get; set; }
        #endregion

        #region CHECK EXIST DATA
        public int ExistData { get; set; }
        #endregion

        public string UserName { get; set; }
    }
}
