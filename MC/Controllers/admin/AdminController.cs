using MC.Models;
using MCComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class AdminController : BaseController
    {
 
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Course()
        {
            var dt = Project.GetList();
            ViewBag.Source = dt;
            return View();
        }


        public ActionResult CourseAdd()
        {
            if (LUser != null)
            {
                if (Request.RequestContext.RouteData.Values["id"] != null)
                {
                    var model = Project.TryFind(Request.RequestContext.RouteData.Values["id"].ToString().ToGuid());
                    if (model != null)
                    {
                        ViewBag.Title = "课程修改";
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Login");
                    }
                }
                else
                {
                    ViewBag.Title = "课程新增";
                    var model = new Project();
                    model.IsUse = false;
                    model.StartDate = model.EndDate = DateTime.Now;
                    model.CrUser = LUser.UserName;
                    model.UpUser = LUser.UserName;
                    model.Amount = 0;
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public ActionResult CourseAdd(Project model)
        {
            if (LUser != null)
            {
                if (model.Name.IsEmpty())
                {
                    ModelState.AddModelError(nameof(model.Name), "请输入课程名称.");
                }
                if (model.Description.IsEmpty())
                {
                    ModelState.AddModelError(nameof(model.Description), "请输入课程描述.");
                }
                if (model.Price == 0)
                {
                    ModelState.AddModelError(nameof(model.Price), "请输入课时单价.");
                }
                if (model.Amount == 0)
                {
                    ModelState.AddModelError(nameof(model.Amount), "请输入课时.");
                }
                if (model.Total == 0)
                {
                    ModelState.AddModelError(nameof(model.Total), "请输入总价.");
                }
                model.UpTime = model.CrTime = DateTime.Now;
                model.CrUserID = LUser.UserId.ToGuid();
                if (ModelState.IsValid)
                {
                    if (model.ID.IsEmpty())
                    {
                        //新增
                        model.ID = GuidHelper.GuidNew();
                        model.CreateAndFlush();
                    }
                    else
                    {
                        model.UpdateAndFlush();
                    }
                    return RedirectToAction("Course", "Admin");
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Order()
        {
            return View();
        }
    }
}