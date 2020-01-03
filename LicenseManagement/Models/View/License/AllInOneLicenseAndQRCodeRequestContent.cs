using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseManagement.Models.View.License
{
    public class AllInOneLicenseAndQRCodeRequestContent
    {
        public List<I3ProductAndSaveLicenseSettingExtCombine> ListCombine { get; set; }
    }

    public class I3ProductAndSaveLicenseSettingExtCombine
    {
        public I3Product I3Product { get; set; }
        public SaveLicenseSettingExt SaveLicenseSetting { get; set; }
    }
}