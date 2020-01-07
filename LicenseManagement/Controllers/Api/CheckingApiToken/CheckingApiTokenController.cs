using LicenseManagement.Authorization;
using LicenseManagement.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LicenseManagement.Controllers.Api.CheckingApiToken
{
    [CustomizeAuthorize]
    public class CheckingApiTokenController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage CheckApiTokenStatus()
        {
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
