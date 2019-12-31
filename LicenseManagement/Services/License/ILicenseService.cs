using LicenseManagement.Models.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseManagement.Services.License
{
    public interface ILicenseService
    {
        string GenerateLicenseAndQRCode(LicenseViewModel licenseVm);
        string GenerateAllInOneLicenseAndQRCode(LicenseViewModel licenseVm);
        IEnumerable<int> GetListI3ProductId();
    }
}
