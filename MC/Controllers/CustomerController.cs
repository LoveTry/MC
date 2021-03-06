﻿using MC.Models;
using MC.Models.query;
using MCComm;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class CustomerController : CommonController
    {
        // GET: Customer
        public ActionResult Add()
        {
            var info = new Customer();

            ViewBag.SexList = new List<SelectListItem>() {
                new SelectListItem() {Text="",Value="" },
                new SelectListItem() { Text="男", Value="男" },
                new SelectListItem() { Text="女", Value="女" },
            };

            ViewBag.ProjectList = Project.GetChooseList();

            return View(info);
        }

        public ActionResult Detail(Guid ID)
        {
            var model = Customer.FindByIdAndCrId(ID, DBUserInfo.UserID.ToGuid());
            if (model != null)
            {
                var query = from row in Order.GetList("CusID='{0}' AND CrUserID='{1}'".FormatWith(ID, DBUserInfo.UserID)).AsEnumerable()
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
                ViewBag.OrderList = query.ToList();

                ViewBag.StateList = BasicInfoContent.GetBasicContent(StateType.OrderSate);

                return View(model);
            }
            else
            {
                FileHelper.WriteLog("ID={0} UID={1}的客户没有查到".FormatWith(ID, DBUserInfo.UserID));
                return RedirectToAction("ErrorPage", "Error", new { msg = "没有查询到客户" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Add(Customer model)
        {
            string[] chooseIdArray = model.ChooseIDList.TrimEnd(',').Split(',');
            if (chooseIdArray.Length == 0)
            {
                return Json(new { code = AjaxReturnCode.WARNNING, msg = "请选择意向课程" });
            }
            if (ModelState.IsValid)
            {
                if (model.ID.IsNotEmpty())
                {
                    Task.Run(() =>
                   {
                       model.UpTime = DateTime.Now;
                       model.Create();
                   });
                }
                else
                {

                    model.ID = GuidHelper.GuidNew();
                    model.CrUserID = DBUserInfo.UserID.ToGuid();
                    model.CrTime = model.UpTime = DateTime.Now;
                    model.CrUser = DBUserInfo.NickName;
                    model.CreateAndFlush();


                    var orderNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                    foreach (var item in chooseIdArray)
                    {
                        var project = Project.Find(item.ToGuid());
                        if (project != null && project.IsUse)
                        {
                            var order = new Order()
                            {
                                OrderNo = orderNo,
                                ProID = project.ID,
                                ProMoney = project.Total,
                                State = "申请中",
                                StateInfo = "正在通知管理员核实",
                                CusID = model.ID,
                                CrTime = DateTime.Now,
                                CrUserID = DBUserInfo.UserID.ToGuid()
                            };
                            order.CreateAndFlush();
                        }
                    }


                    var temp = new 订单生成通知()
                    {
                        first = new TemplateDataItem("您收到新的订单"),
                        keyword1 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                        keyword2 = new TemplateDataItem("推荐人：" + DBUserInfo.NickName),
                        keyword3 = new TemplateDataItem(orderNo),
                        remark = new TemplateDataItem("请登录后台进行审核。")
                    };
                    CommFunction.SendWxTemplateMsg(temp, MessageType.订单生成通知, MPBasicSetting.noticeopid);
                }
            }
            return Json(new { url = "/Center/Index/2" });

        }

        public ActionResult ModifyCus()
        {
            string id = HttpContext.Request.Params["id"];
            string cusName = HttpContext.Request.Params["cusName"];
            string cusPhone = HttpContext.Request.Params["cusPhone"];
            Customer cus = Customer.TryFind(id.ToGuid());
            if (cus != null)
            {
                cus.CusName = cusName;
                cus.CusPhone = cusPhone;
                cus.UpTime = DateTime.Now;
                cus.UpdateAndFlush();
                return Json(new { url = "/Customer/Detail/" + cus.ID }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { url = "/Center" }, JsonRequestBehavior.AllowGet); ;
        }

        public async Task<ActionResult> CusListAsync()
        {
            var s = DateTime.Now;
            ViewBag.Num = 10;
            var a = Task.Run(() =>
             {
                 Thread.Sleep(5);
             });

            var b = Task.Run(() =>
            {
                Thread.Sleep(5);
            });
            await a;
            await b;

            var e = DateTime.Now;
            var t = (e - s).Milliseconds;
            ViewBag.Time = t;
            return View("CusList");
        }

        public ActionResult CusList()
        {
            ViewBag.Num = 10;
            return View();
        }


    }
}