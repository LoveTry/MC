using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Models
{
    public class LoginUser
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }

        public static LoginUser GetLoginUser(Guid userid)
        {
            var loginer = new LoginUser()
            {
                Role = "",
                UserId = "88888888-8888-8888-8888-888888888888",
                UserName = "Admin"
            };
            HttpContext.Current.Session.Add(Const.PC_USRE_INFO, loginer);
            return loginer;
        }
    }
}