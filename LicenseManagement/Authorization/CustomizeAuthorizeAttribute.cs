using LicenseManagement.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace LicenseManagement.Authorization
{
    public class CustomizeAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated == false)
                {
                    var logger = new Logger();
                    var origin = actionContext.Request.Headers.Referrer?.GetLeftPart(UriPartial.Authority);
                    if (origin != null)
                    {
                        logger.Info("Connection Origin", origin);
                    }
                }
            }
            catch { }
        }
    }
}