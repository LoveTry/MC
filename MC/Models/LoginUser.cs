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

        public static LoginUser GetLoginUser(string username, string password)
        {
            var adminUser = AdminUser.GetAdminUser(username, password);
            if (adminUser != null)
            {
                var loginer = new LoginUser()
                {
                    Role = "",
                    UserId = adminUser.ID.ToString(),
                    UserName = adminUser.UserName
                };
                HttpContext.Current.Session.Add(Const.PC_USRE_INFO, loginer);
                //更新登录时间
                adminUser.LastOnLineTime = DateTime.Now;
                adminUser.UpdateAndFlush();
                return loginer;
            }
            else
            {
                HttpContext.Current.Session[Const.PC_USRE_INFO] = null;
                return null;
            }
        }
    }
}