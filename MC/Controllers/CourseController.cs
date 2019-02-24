using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }
    }
}