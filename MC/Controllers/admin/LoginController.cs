using MC.Models;
using MCComm;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            var json = Request.Form["data"];
            JObject jo = JObject.Parse(json);
            var username = jo.GetValue("username").ToString();
            var password = jo.GetValue("password").ToString();
            if (username.IsNotEmpty() && password.IsNotEmpty())
            {
                var logUser = LoginUser.GetLoginUser(username, password);
                if (logUser != null)
                {
                    return Content("/admin");
                }
            }
            return Content("failed");
        }
    }
}