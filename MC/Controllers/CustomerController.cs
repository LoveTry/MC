﻿using System;
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
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }
    }
}