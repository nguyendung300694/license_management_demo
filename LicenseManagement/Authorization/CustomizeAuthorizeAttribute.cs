﻿using LicenseManagement.Services.Logger;
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

            LogConnectionOrigin(actionContext);
        }

        private void LogConnectionOrigin(HttpActionContext actionContext)
        {
            try
            {
                //if (HttpContext.Current.User.Identity.IsAuthenticated == false)
                //{
                var logger = new Logger();
                var origin = actionContext.Request.Headers.Referrer?.GetLeftPart(UriPartial.Authority);
                logger.Info("Connection Origin", origin ?? "N/A");
                //}
            }
            catch (Exception ex)
            {
                var logger = new Logger();
                logger.Error("Connection Origin", ex.Message);
            }
        }
    }
}