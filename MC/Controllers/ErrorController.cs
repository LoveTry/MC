using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult ErrorPage(string msg = "请使用微信访问")
        {
            ViewBag.ErrorMsg = msg;
            return View();
        }
    }
}