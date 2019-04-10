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
            //Session.Clear();
            if (Session[Const.PC_USRE_INFO] != null)
            {
                //Session里存在 说明登录过
                lUser = Session[Const.PC_USRE_INFO] as LoginUser;
            }
            else
            {
#if DEBUG
                LoginUser.GetLoginUser("admin", "e6f59560edc55d422647b50df2ed6113");
#else
                filterContext.Result = RedirectToAction("index", "login");
#endif
            }
        }
    }
}