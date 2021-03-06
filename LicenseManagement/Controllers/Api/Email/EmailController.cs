﻿using LicenseManagement.Authorization;
using LicenseManagement.Models.View.Email;
using LicenseManagement.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LicenseManagement.Controllers.Api.Email
{
    [CustomizeAuthorize(Roles = AppSettings.UserRole.Admin)]
    public class EmailController : ApiController
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [AllowAnonymous]
        public string PostEmailToSend(EmailViewModel emailModel)
        {
            //return Guid.NewGuid().ToString();
            return _emailService.SendEmail(emailModel);
        }
    }
}
