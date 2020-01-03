using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LicenseManagement.Models.View.Account;

namespace LicenseManagement.Services.Account.CustomUser
{
    public class CustomUserService : ICustomUserService
    {
        private List<UserCustom> _listUser = new List<UserCustom>();

        public CustomUserService()
        {

            _listUser.Add(new UserCustom
            {
                UserName = AppSettings.LicenseManagementUserName.Trim(),
                Password = AppSettings.LicenseManagementPassword.Trim(),
                ListRoles = new List<string>
                {
                    AppSettings.UserRole.Admin
                },
                Id = 1
            });
        }

        public UserCustom GetUserByUserNameAndPassword(string userName, string passWord)
        {
            return _listUser.FirstOrDefault(m => m.UserName == userName?.Trim() && m.Password == passWord?.Trim());
        }
    }
}