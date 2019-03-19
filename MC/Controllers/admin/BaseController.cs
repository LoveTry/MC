using MC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MCComm;

namespace MC.Controllers
{
    public class BaseController : Controller
    {

        private LoginUser lUser = null;
        /// <summary>
        /// 用户信息
        /// </summary>
        protected LoginUser LUser
        {
            get
            {
                if (lUser == null)
                {
                    if (Session[Const.PC_USRE_INFO] != null)
                    {
                        lUser = Session[Const.PC_USRE_INFO] as LoginUser;
                    }
                }
                return lUser;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Session[Const.PC_USRE_INFO] != null)
            {
                //Session里存在 说明登录过
                lUser = Session[Const.PC_USRE_INFO] as LoginUser;
            }
            else
            {
                LoginUser.GetLoginUser("88888888-8888-8888-8888-888888888888".ToGuid());
                //filterContext.Result = RedirectToAction("Login", "Login", new { msg = "该页面已经过期，请退出当前菜单重新进入！" });
            }
        }
    }
}