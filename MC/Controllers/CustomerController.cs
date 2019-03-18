using MC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Add()
        {
            var info = new Customer();

            ViewBag.Sex = new List<SelectListItem>() {
                new SelectListItem() {Text="",Value="" },
                new SelectListItem() { Text="男", Value="男" },
                new SelectListItem() { Text="女", Value="女" },
            };

            ViewBag.ProjectList = Project.GetChooseList();

            return View(info);
        }

        public ActionResult Detail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Customer model)
        {
            if (ModelState.IsValid)
            {

            }
            return RedirectToAction("CusList");
          
        }

        public ActionResult CusList()
        {
            return View();
        }
    }
}