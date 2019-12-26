using LicenseManagement.Models.License;
using LicenseManagement.Services.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LicenseManagement.Controllers.Api
{
    //[EnableCors(origins: "*", headers: "*",
    //methods: "*", SupportsCredentials = true)]
    public class LicenseController : ApiController
    {
        private readonly ILicenseService _licenseService;

        public LicenseController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpPost]
        public HttpResponseMessage PostQRCode(LicenseViewModel licenseVM)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _licenseService.GenerateLicenseAndQRCode(licenseVM));
        }

        [HttpPost]
        public HttpResponseMessage PostAllInOneQRCode(LicenseViewModel licenseVM)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _licenseService.GenerateAllInOneLicenseAndQRCode(licenseVM));
        }
    }
}
