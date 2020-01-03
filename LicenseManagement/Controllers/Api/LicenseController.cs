using LicenseManagement.Models.View.License;
using LicenseManagement.Services.License;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LicenseManagement.Controllers.Api
{
    //[EnableCors(origins: "*", headers: "*",
    //methods: "*", SupportsCredentials = true)]
    [Authorize]
    public class LicenseController : ApiController
    {
        private readonly ILicenseService _licenseService;

        public LicenseController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpPost]
        public HttpResponseMessage PostQRCode(LicenseViewModel licenseVm)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _licenseService.GenerateLicenseAndQRCode(licenseVm));
        }

        [HttpPost]
        public HttpResponseMessage PostAllInOneQRCode(JObject objData)
        {
            dynamic jsonData = objData;
            var licenseVm = ((JObject)jsonData.licenseVm).ToObject<LicenseViewModel>();
            var dataContent = ((JObject)jsonData.dataContent).ToObject<AllInOneLicenseAndQRCodeRequestContent>();
            return Request.CreateResponse(HttpStatusCode.OK, _licenseService.GenerateAllInOneLicenseAndQRCode(licenseVm, dataContent));
        }

        [HttpGet]
        public HttpResponseMessage GetListI3Product()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _licenseService.GetListI3Product());
        }

        public HttpResponseMessage GetLicenseFile(string fileUrl, string serialCode)
        {
            var extension = Path.GetExtension(fileUrl);
            var fullFilePath = System.Web.HttpContext.Current.Server.MapPath("~" + fileUrl);
            var fileStream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read);

            //var nameFile = Convert.ToString(serialCode) + "-" +
            //               string.Format("{0:D2}-{1:D2}", DateTime.Now.Month, DateTime.Now.Day) + extension;
            var nameFile = Path.GetFileName(fullFilePath);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(fileStream);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = nameFile;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(string.Format("application/{0}", extension));

            return response;
        }
    }
}
