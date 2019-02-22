using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using MC.Models;

namespace MC.Controllers
{
    public class CommonController : Controller
    {
        private DBUserInfo dbUserInfo = null;
        /// <summary>
        /// 用户信息
        /// </summary>
        protected DBUserInfo DBUserInfo
        {
            get
            {
                if (dbUserInfo == null)
                {
                    if (Session[Const.SESSION_USRE_INFO] != null)
                    {
                        dbUserInfo = Session[Const.SESSION_USRE_INFO] as DBUserInfo;
                    }
                }
                return dbUserInfo;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Session[Const.SESSION_USRE_INFO] != null)
            {
                //Session里存在 说明登录过
                dbUserInfo = Session[Const.SESSION_USRE_INFO] as DBUserInfo;
            }
            else
            {
                //获取参数
                if (Request.QueryString["code"] != null)
                {
                    //从菜单进入
                    string code = Request.QueryString["code"];
                    string openid = string.Empty;
                    dbUserInfo = CommFunction.GetMPUserInfo(code, out openid);
                    if (dbUserInfo == null)
                    {
                        filterContext.Result = RedirectToAction("UserBing", "Account", new { IsSkip = 1, openid });
                    }
                }
                else if (Request.QueryString["openid"] != null)
                {
                    string openid = Request.QueryString["openid"];
                    //模板消息进入
                    dbUserInfo = Models.DBUserInfo.GetUserDBInfo(openid);
                }
                else
                {
#if DEBUG
                    dbUserInfo = Models.DBUserInfo.GetUserDBInfo(System.Web.Configuration.WebConfigurationManager.AppSettings["testOpenId"]);
#else
                    filterContext.Result = RedirectToAction("ErrorPage", "Error", new { msg = "该页面已经过期，请退出当前菜单重新进入！" });
                    //dbUserInfo = Models.DBUserInfo.GetUserDBInfo(System.Web.Configuration.WebConfigurationManager.AppSettings["testOpenId"]);
#endif
                }
            }
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult ErrorPage(string msg = "该页面已经过期，请退出当前菜单重新进入！")
        {
            return RedirectToAction("ErrorPage", "Error", new { msg });
        }

        /// <summary>
        /// 获取绑定用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetContactList()
        {
            if (this.DBUserInfo == null)
            {
                return ErrorPage();
            }
            DataTable dt = new DataTable();
            var query = from row in dt.AsEnumerable()
                        select new
                        {
                            UserId = row.Field<Guid>("UserID"),
                            Truename = row.Field<string>("TrueName"),
                            Img = CommFunction.HeadImage(row.Field<string>("WxOpenID")),
                            DeptName = row.Field<string>("DeptName")
                        };
            return Json(query.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取JS-SDK权限验证签名
        /// </summary>
        protected void GetgetJsApiTicket()
        {
            ViewBag.appId = MPBasicSetting.AppID;
            string ticket = Senparc.Weixin.MP.Containers.JsApiTicketContainer.GetJsApiTicket(MPBasicSetting.AppID);
            string nonceStr = Senparc.Weixin.MP.Helpers.JSSDKHelper.GetNoncestr();
            string timestamp = Senparc.Weixin.MP.Helpers.JSSDKHelper.GetTimestamp();
            ViewBag.nonceStr = nonceStr;
            ViewBag.timestamp = timestamp;
            string url = Request.Url.ToString().Replace(":" + Request.Url.Port, "");
            string signature = Senparc.Weixin.MP.Helpers.JSSDKHelper.GetSignature(ticket, nonceStr, timestamp, url);
            ViewBag.signature = signature;
            ViewBag.BaseUrl = MPBasicSetting.wxUrl;

        }
    }
}
