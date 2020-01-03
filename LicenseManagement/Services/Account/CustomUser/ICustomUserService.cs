using LicenseManagement.Models.Entity;
using LicenseManagement.Models.View.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseManagement.Services.Account.CustomUser
{
    public interface ICustomUserService
    {
        UserCustom GetUserByUserNameAndPassword(string userName, string passWord);
    }
}
