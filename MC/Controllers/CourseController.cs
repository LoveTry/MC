using MC.Models;
using MCComm;
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
        public ActionResult Detail(string id)
        {
            Project model = Project.TryFind(id.ToGuid());
            ProjectIntroduce info = ProjectIntroduce.TryFind(id.ToGuid());
            if (info != null)
            {
                ViewBag.Content = info.Content;
            }
            return View(model);
        }

        public ActionResult List()
        {
            return View();
        }
    }
}