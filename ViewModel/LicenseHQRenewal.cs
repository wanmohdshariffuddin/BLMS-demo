using BLMS.Models.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.ViewModel
{
    public class LicenseHQRenewal
    {
        public LicenseHQ RenewalLicense { get; set; }

        public List<LicenseHQ> LicenseLog { get; set; }
    }
}
