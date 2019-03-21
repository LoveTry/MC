using MC.Controllers;
using MC.Models;
using MC.Models.query;
using MCComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class CenterController : CommonController
    {
        // GET: Center
        public ActionResult Index()
        {
            ViewBag.UserName = DBUserInfo.NickName;
            ViewBag.HeadImageUrl = CommFunction.HeadImage(DBUserInfo.openID);
            ViewBag.CompanyName = "佣金0.00元";
            ViewBag.openid = DBUserInfo.openID;
            //默认激活那个页面
            Actived(Request.RequestContext.RouteData.Values["id"] != null ? Request.RequestContext.RouteData.Values["id"].ToString().ToInt() : 1);

            //客户列表数据
            var queryCustomerList = from row in Customer.GetCustomerList("CrUserID='{0}'".FormatWith(DBUserInfo.UserID)).AsEnumerable()
                                    select new CustomerQuery
                                    {
                                        ID = row.Field<Guid>("ID"),
                                        Name = row.Field<string>("CusName"),
                                        Num = row.Field<int>("OrderNum"),
                                        Phone = row.Field<string>("CusPhone"),
                                        Sex = row.Field<string>("Sex")
                                    };
            ViewBag.CustomerList = queryCustomerList.ToList();
            return View();
        }

        public ActionResult MyCommission(string openid)
        {
            return View();
        }


        /// <summary>
        /// 默认激活Tab
        /// </summary>
        /// <param name="index"></param>
        private void Actived(int index = 1)
        {
            #region
            switch (index)
            {
                case 1:
                    ViewBag.Actived1 = "weui-tab__bd-item--active";
                    ViewBag.Actived2 = "";
                    ViewBag.Actived3 = "";
                    ViewBag.Actived4 = "";

                    ViewBag.TabActived1 = "weui-bar__item--on";
                    ViewBag.TabActived2 = "";
                    ViewBag.TabActived3 = "";
                    ViewBag.TabActived4 = "";
                    break;
                case 2:
                    ViewBag.Actived1 = "";
                    ViewBag.Actived2 = "weui-tab__bd-item--active";
                    ViewBag.Actived3 = "";
                    ViewBag.Actived4 = "";

                    ViewBag.TabActived1 = "";
                    ViewBag.TabActived2 = "weui-bar__item--on";
                    ViewBag.TabActived3 = "";
                    ViewBag.TabActived4 = "";
                    break;
                case 3:
                    ViewBag.Actived1 = "";
                    ViewBag.Actived2 = "";
                    ViewBag.Actived3 = "weui-tab__bd-item--active";
                    ViewBag.Actived4 = "";

                    ViewBag.TabActived1 = "";
                    ViewBag.TabActived2 = "";
                    ViewBag.TabActived3 = "weui-bar__item--on";
                    ViewBag.TabActived4 = "";
                    break;
                case 4:
                    ViewBag.Actived1 = "";
                    ViewBag.Actived2 = "";
                    ViewBag.Actived3 = "";
                    ViewBag.Actived4 = "weui-tab__bd-item--active";

                    ViewBag.TabActived1 = "";
                    ViewBag.TabActived2 = "";
                    ViewBag.TabActived3 = "";
                    ViewBag.TabActived4 = "weui-bar__item--on";
                    break;
                default:
                    ViewBag.Actived1 = "weui-tab__bd-item--active";
                    ViewBag.Actived2 = "";
                    ViewBag.Actived3 = "";
                    ViewBag.Actived4 = "";

                    ViewBag.TabActived1 = "weui-bar__item--on";
                    ViewBag.TabActived2 = "";
                    ViewBag.TabActived3 = "";
                    ViewBag.TabActived4 = "";
                    break;
            }
            #endregion
        }

        public ActionResult CreateQrcode()
        {
            string url = Comm.QrCodeCreater.CreateQrCode("");
            return Redirect(url);
        }
    }
}