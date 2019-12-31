using LicenseManagement.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseManagement.Services.Email
{
    public interface IEmailService
    {
        string SendEmail(EmailViewModel emailModel);
    }
}
