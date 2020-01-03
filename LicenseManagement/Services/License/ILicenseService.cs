using LicenseManagement.Models.View.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseManagement.Services.License
{
    public interface ILicenseService
    {
        LicenseAndQRCodeResponse GenerateLicenseAndQRCode(LicenseViewModel licenseVm);
        LicenseAndQRCodeResponse GenerateAllInOneLicenseAndQRCode(LicenseViewModel licenseVm, AllInOneLicenseAndQRCodeRequestContent dataContent);
        IEnumerable<I3Product> GetListI3Product();
    }
}
