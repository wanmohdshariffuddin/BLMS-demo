using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Models.Admin
{
    public class UserRole
    {
        [Key]
        public int UserRoleID { get; set; }

        public string OldUserRoleStaffNo { get; set; }

        public int OldRoleID { get; set; }

        public int OldUserTypeID { get; set; }

        [DisplayName("STAFF NO")]
        public string UserRoleStaffNo { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("STAFF NAME")]
        public string UserRoleName { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("STAFF EMAIL")]
        public string UserRoleEmail { get; set; }

        public int RoleID { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("ROLE")]
        public string Role { get; set; }

        public int UserTypeID { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        [DisplayName("USER TYPE")]
        public string UserType { get; set; }

        //check linked data
        public int ExistData { get; set; }

        public string UserName { get; set; }
    }
}
