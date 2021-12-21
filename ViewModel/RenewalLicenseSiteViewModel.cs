using BLMS.Models.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.ViewModel
{
    public class RenewalLicenseSiteViewModel
    {
        public LicenseSite RenewalLicense { get; set; }

        public List<LicenseSite> History { get; set; }
    }
}
