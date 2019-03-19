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
            model.TypeID = BasicInfoType.GetMaxTypeID() + 1;
            model.CrUser = LUser.UserName;
            model.UpUser = LUser.UserName;
            return View(model);
        }

        [HttpPost]
        public ActionResult TypeCodeAdd(BasicInfoType model)
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
                        model.UpTime= DateTime.Now;
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