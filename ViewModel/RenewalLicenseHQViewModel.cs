using BLMS.Models.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.ViewModel
{
    public class RenewalLicenseHQViewModel
    {
        public LicenseAdmin RenewalLicense { get; set; }

        public List<LicenseAdmin> History { get; set; }
    }
}
