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
            return View(info);
        }

        public ActionResult Detail()
        {
            return View();
        }
    }
}