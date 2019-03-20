using MC.Models;
using MCComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class BasicCodeController : BaseController
    {

        public ActionResult BasicContentList()
        {
            var query = from row in BasicInfoContent.GetBasicContentList().AsEnumerable()
                        select new BasicInfoContent
                        {
                            CrTime = row.Field<DateTime>("CrTime"),
                            CrUser = row.Field<string>("CrUser"),
                            CrUserId = row.Field<Guid>("CrUserId"),
                            DelFlag = row.Field<bool>("DelFlag"),
                            ID = row.Field<int>("ID"),
                            TypeID = row.Field<int>("TypeID"),
                            TypeName = row.Field<string>("TypeName"),
                            UpTime = row.Field<DateTime>("UpTime"),
                            UpUser = row.Field<string>("UpUser"),
                            BasicTypeName = row.Field<string>("BasicTypeName"),
                            SequenceOrder = row.Field<int>("SequenceOrder")
                        };
            ViewBag.Source = query.ToList();
            return View();
        }

        public ActionResult BasicContentAdd()
        {

            ViewBag.BasicTypeList = BasicInfoType.GetTypeList();
            if (Request.RequestContext.RouteData.Values["id"] != null)
            {
                ViewBag.Title = "基本代码修改";
                var model = BasicInfoContent.TryFind(Request.RequestContext.RouteData.Values["id"].ToString().ToInt());
                return View(model);
            }
            else
            {
                var model = new BasicInfoContent();
                model.ID = 0;
                model.DelFlag = false;
                model.TypeID = BasicInfoContent.GetMaxSeq() + 1;
                model.CrUser = LUser.UserName;
                model.UpUser = LUser.UserName;
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult BasicContentAdd(BasicInfoContent model)
        {
            if (model.TypeName.IsEmpty())
            {
                ModelState.AddModelError(nameof(model.TypeName), "请输入名称");
            }
            else if (model.TypeID == 0)
            {
                ModelState.AddModelError(nameof(model.TypeID), "请输入类型");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (model.ID == 0)
                    {
                        //新增
                        model.CrTime = model.UpTime = DateTime.Now;
                        model.CrUserId = LUser.UserId.ToGuid();
                        model.CreateAndFlush();
                    }
                    else
                    {
                        model.UpTime = DateTime.Now;
                        model.UpUser = LUser.UserName;
                        model.UpdateAndFlush();
                    }
                    return RedirectToAction("BasicContentList");
                }
            }
            return View(model);
        }

        // GET: BasicCode
        public ActionResult BasicTypeList()
        {

            var query = from row in BasicInfoType.GetList().AsEnumerable()
                        select new BasicInfoType
                        {
                            CrTime = row.Field<DateTime>("CrTime"),
                            CrUser = row.Field<string>("CrUser"),
                            CrUserId = row.Field<Guid>("CrUserId"),
                            DelFlag = row.Field<bool>("DelFlag"),
                            ID = row.Field<int>("ID"),
                            TypeID = row.Field<int>("TypeID"),
                            TypeName = row.Field<string>("TypeName"),
                            UpTime = row.Field<DateTime>("UpTime"),
                            UpUser = row.Field<string>("UpUser")
                        };
            ViewBag.Source = query.ToList();
            return View();
        }

        public ActionResult BasicTypeAdd()
        {
            var model = new BasicInfoType();
            model.ID = 0;
            model.DelFlag = false;
            model.TypeID = BasicInfoType.GetMaxTypeID() + 1;
            model.CrUser = LUser.UserName;
            model.UpUser = LUser.UserName;
            return View(model);
        }

        [HttpPost]
        public ActionResult BasicTypeAdd(BasicInfoType model)
        {
            if (model.TypeName.IsEmpty())
            {
                ModelState.AddModelError(nameof(model.TypeName), "请输入类型名称");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (model.ID == 0)
                    {
                        //新增
                        model.CrTime = model.UpTime = DateTime.Now;
                        model.CrUserId = LUser.UserId.ToGuid();
                        model.CreateAndFlush();
                    }
                    else
                    {
                        model.UpTime = DateTime.Now;
                        model.UpUser = LUser.UserName;
                        model.UpdateAndFlush();
                    }
                    return RedirectToAction("BasicTypeList");
                }
            }
            return View(model);
        }
    }
}