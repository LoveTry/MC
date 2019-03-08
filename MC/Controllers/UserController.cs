using MC.Models;
using MCComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class UserController : CommonController
    {
        // GET: User
        public ActionResult Update()
        {
            ViewBag.Sex = new List<SelectListItem>() {
                new SelectListItem() {Text="",Value="" },
                new SelectListItem() { Text="男", Value="男" },
                new SelectListItem() { Text="女", Value="女" },
            };
            var user = Models.User.TryFind(DBUserInfo.UserID.ToGuid());
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(User model)
        {
            if (TryValidateModel(model))
            {
                model.Update();
                DBUserInfo.CleearUserSession();
            }
            return RedirectToAction("Index", "Center", new { Actived = 4 });
        }


    }
}