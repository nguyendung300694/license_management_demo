using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseManagement.Models.View.License
{
    public class LicenseAndQRCodeResponse
    {
        public string RelativePath { get; set; }
        public List<LicenseViewModelAndLicensePathCombine> ListLicenseVmAndLicensePathCombine { get; set; }
    }

    public class LicenseViewModelAndLicensePathCombine
    {
        public LicenseViewModel LicenseVm { get; set; }
        public string LicensePath { get; set; }
    }
}