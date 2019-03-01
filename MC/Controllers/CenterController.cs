using MC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class CenterController : CommonController
    {
        // GET: Center
        public ActionResult Index()
        {
            ViewBag.UserName = DBUserInfo.NickName;
            ViewBag.HeadImageUrl = CommFunction.HeadImage(DBUserInfo.openID);
            ViewBag.CompanyName = "佣金0.00元";
            ViewBag.openid = DBUserInfo.openID;
            return View();
        }

        public ActionResult MyCommission(string openid)
        {
            return View();
        }
    }
}