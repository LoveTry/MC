using MC.Comm;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class BaseCommController : Controller
    {
        public string OpenId = string.Empty;
        public string OK(object data)
        {
            var obj = new { ok = 1, data };
            return JsonConvert.SerializeObject(obj);
        }

        public string OK()
        {
            var obj = new { ok = 1 };
            return JsonConvert.SerializeObject(obj);
        }

        public string Error(string msg)
        {
            var obj = new { ok = 0, msg };
            return JsonConvert.SerializeObject(obj);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                //OpenId = "oNcSFwaaIlLenUtee-4GAN3V--Vg";
                base.OnActionExecuting(filterContext);
                //获取参数
                if (Request.QueryString["code"] != null)
                {
                    //从菜单进入
                    string code = Request.QueryString["code"];
                    var oauthTokenInfo = OAuthApi.GetAccessToken(MPBasicSetting.AppID, MPBasicSetting.AppSecret, code);
                    OpenId = oauthTokenInfo.openid;
                }
                else if (Request.QueryString["openid"] != null)
                {
                    OpenId = Request.QueryString["openid"];
                }
                else
                {
#if DEBUG
                    OpenId = System.Web.Configuration.WebConfigurationManager.AppSettings["testOpenId"];
#else
                    filterContext.Result = RedirectToAction("ErrorPage", "Error", new { msg = "请用微信访问！" });
#endif
                }
            }
            catch
            {
                filterContext.Result = RedirectToAction("ErrorPage", "Error", new { msg = "授权过期！" });
            }
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

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileUpload">The file upload.</param>
        /// <param name="message">The message.</param>
        /// <param name="suffixList">The suffix list.</param>
        /// <returns></returns>
        protected bool SaveFile(string path, string fileName, byte[] imgStream)
        {
            try
            {
                if (!Directory.Exists(path))                    //保存文件的目录是否存在
                {
                    Directory.CreateDirectory(path);
                }

                System.IO.File.WriteAllBytes(Path.Combine(path, fileName), imgStream);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log("SaveFile:" + ex.Message);
                return false;
            }
        }
    }
}