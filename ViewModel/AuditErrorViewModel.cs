using BLMS.Models.Admin;
using System.Collections.Generic;

namespace BLMS.ViewModel
{
    public class AuditErrorViewModel
    {
        public List<ErrorLog> errorLog { get; set; }

        public List<AuditLog> auditLog { get; set; }
    }
}
