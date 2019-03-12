using MC.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class ADController : Controller
    {
        // GET: AD
        public ActionResult Ad(int id)
        {
            switch (id)
            {
                case (int)CustomerAgent.YILICHANG:
                    ViewBag.Content = "我是亿利昶广告";
                    break;
                case (int)CustomerAgent.LIN:
                    ViewBag.Content = "我是艺利广告";
                    break;
            }
            return View();
        }
    }
}