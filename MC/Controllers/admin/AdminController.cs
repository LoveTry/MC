using MC.Models;
using MCComm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using MC.Models.query;

namespace MC.Controllers
{
    public class AdminController : BaseController
    {

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.UserName = LUser.UserName;
            var obj = CommFunction.GetArrayByDt(ReportTotal.GetProNameAndFee());
            //报表设置
            ViewBag.LegendArray = obj["Name"];
            ViewBag.SeriesArray = obj["Fee"];
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("index", "login");
        }

        #region Course
        public ActionResult Course()
        {
            var query = new GeneralQuery();
            return View(query);
        }

        public JsonResult CourseListJson(int page, int limit)
        {
            var where = GeneralQuery.Get(Request.QueryString);
            var query = Project.GetList(where);
            var data = new JsonReturn(query, page, limit);
            return Json(data, JsonRequestBehavior.AllowGet);
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
                        return Json(JsonReturn.Error("查询数据失败"), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.Title = "课程新增";
                    var model = new Project();
                    model.IsUse = true;
                    model.StartDate = model.EndDate = DateTime.Now;
                    model.CrUser = LUser.UserName;
                    model.CrTime = model.UpTime = DateTime.Now;
                    model.UpUser = LUser.UserName;
                    model.Amount = 0;
                    return View(model);
                }
            }
            else
            {
                return Json(JsonReturn.Error("登录超时，请重新登录"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CourseAdd(Project model)
        {
            if (LUser != null)
            {
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
                    return Json(JsonReturn.OK());
                }
                else
                {
                    return Json(JsonReturn.Error("填写的信息有误请确认。"), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonReturn.Error("登录超时，请重新登录"), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CourseDel(string id, bool isUse)
        {
            if (LUser != null)
            {
                var model = Project.TryFind(id.ToGuid());
                if (model != null)
                {
                    model.IsUse = isUse;
                    model.UpUser = LUser.UserName;
                    model.UpTime = DateTime.Now;
                    model.UpdateAndFlush();
                    return Json(JsonReturn.OK(),JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonReturn.Error("查询数据失败"), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonReturn.Error("登录超时，请重新登录"), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Order
        public ActionResult OrderList()
        {
            return View();
        }

        public JsonResult OrderListJson(int page, int limit)
        {
            var query = from row in Order.GetList("1=1").AsEnumerable()
                        select new OrderQuery
                        {
                            CrTime = row.Field<DateTime>("CrTime").ToString("yyyy-MM-dd"),
                            CusName = row.Field<string>("CusName"),
                            CusPhone = row.Field<string>("CusPhone"),
                            ID = row.Field<int>("ID"),
                            Name = row.Field<string>("Name"),
                            OrderNo = row.Field<string>("OrderNo"),
                            ProMoney = row.Field<decimal>("ProMoney"),
                            State = row.Field<string>("State"),
                            StateInfo = row.Field<string>("StateInfo"),
                            TrueName = row.Field<string>("TrueName"),
                            UserName = row.Field<string>("UserName")
                        };
            var data = new JsonReturn(query, page, limit);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OrderConfirm(int id, bool isOK, string msg)
        {
            var info = Order.TryFind(id);
            if (info != null)
            {
                if (isOK)
                {
                    decimal fee = 0;
                    var feeRate = MC.Models.FeeRate.FindFirst();
                    if (feeRate != null)
                    {
                        fee = info.ProMoney * feeRate.Rate - feeRate.Other;
                    }
                    info.State = "结佣中";
                    info.StateID = 2;
                    info.StateInfo = "管理员已经审核完成，等待结佣{0}元".FormatWith(fee.ToMoney(2));
                    info.UpdateAndFlush();

                    var feeInfo = new Fee();
                    feeInfo.ID = GuidHelper.GuidNew();
                    feeInfo.OrderID = info.ID;
                    feeInfo.Money = fee;
                    feeInfo.IsPay = false;
                    feeInfo.PayeeID = info.CrUserID;
                    feeInfo.CrUserID = LUser.UserId.ToGuid();
                    feeInfo.CrUser = LUser.UserName;
                    feeInfo.CrTime = DateTime.Now;
                    feeInfo.CreateAndFlush();
                }
                else
                {
                    info.State = "被驳回";
                    info.StateInfo = msg;
                    info.StateID = 5;
                    info.UpdateAndFlush();
                }

                var approve = new ApproveDetail()
                {
                    ApproveMsg = msg,
                    Approver = LUser.UserName,
                    ApproveTime = DateTime.Now,
                    ApproveType = (int)ApproveType.ORDER,
                    BusinessIntID = id,
                    IsOK = isOK
                };
                approve.CreateAndFlush();
            }
            return Json(JsonReturn.OK(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FeeList
        public ActionResult FeeList()
        {
            return View();
        }

        public JsonResult FeeListJson(int page, int limit)
        {
            var query = from row in Fee.GetFeeListAll().AsEnumerable()
                        select new Fee
                        {
                            Approver = row.Field<string>("Approver"),
                            ApproverID = row.Field<Guid>("ApproverID"),
                            CrTime = row.Field<DateTime>("CrTime"),
                            CrUser = row.Field<string>("CrUser"),
                            CrUserID = row.Field<Guid>("CrUserID"),
                            ID = row.Field<Guid>("ID"),
                            IsPay = row.Field<bool>("IsPay"),
                            Money = row.Field<decimal>("Money"),
                            OrderID = row.Field<int>("OrderID"),
                            OrderNo = row.Field<string>("OrderNo"),
                            PayeeID = row.Field<Guid>("PayeeID"),
                            PayeeTrueName = row.Field<string>("PayeeTrueName"),
                            PayeeUserName = row.Field<string>("PayeeUserName"),
                            ApproveTime = row.Field<DateTime?>("ApproveTime")
                        };
            var data = new JsonReturn(query, page, limit);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 佣金费率
        public ActionResult FeeRate()
        {
            var info = Models.FeeRate.FindOne();
            if (info == null)
            {
                info = new FeeRate();
                info.Rate = 0;
                info.Other = 0;
                info.CrUser = info.UpUser = LUser.UserName;
            }
            return View(info);
        }

        [HttpPost]
        public JsonResult FeeRate(FeeRate model)
        {
            if (ModelState.IsValid)
            {
                if (model.ID.IsEmpty())
                {
                    model.ID = GuidHelper.GuidNew();
                    model.CrUserId = LUser.UserId.ToGuid();
                    model.UpTime = model.CrTime = DateTime.Now;
                    model.CreateAndFlush();
                }
                else
                {
                    model.UpTime = DateTime.Now;
                    model.UpUser = LUser.UserName;
                    model.UpdateAndFlush();
                }
            }
            return Json(JsonReturn.OK(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}