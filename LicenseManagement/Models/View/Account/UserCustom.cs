using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseManagement.Models.View.Account
{
    public class UserCustom
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> ListRoles { get; set; }
    }

}