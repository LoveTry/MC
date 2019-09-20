using MCComm;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MPMenu.CreateMenuDefault();
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];
            if (Request.HttpMethod == "GET")
            {
                //仅在微信后台填写URL验证时触发
                if (MPBasicSetting.FirstCertification == "1")
                {
                    if (CheckSignature.Check(signature, timestamp, nonce, MPBasicSetting.Token))
                    {
                        FileHelper.WriteLog(echostr);
                        return Content(echostr);
                    }
                }
            }
            else
            {
                //post method - 当有用户向公众账号发送消息时触发
#if !DEBUG
                if (!CheckSignature.Check(signature, timestamp, nonce, MPBasicSetting.Token))
                {
                    //WriteContent("服务器忙！");
                    FileHelper.WriteLog("Token验证失败", signature + " " + MPBasicSetting.Token);
                    return Content("服务器忙...");
                }
#endif

                //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
                var maxRecordCount = 10;

                try
                {
                    //当使用加密模式时，此参数有用。
                    var pm = new PostModel()
                    {
                        Signature = signature,
                        Msg_Signature = Request.QueryString["msg_signature"],
                        Timestamp = timestamp,
                        Nonce = nonce,
                        //以下保密信息不会（不应该）在网络上传播，请注意
                        Token = MPBasicSetting.Token,
                        //EncodingAESKey = "mNnY5GekpChwqhy2c4NBH90g3hND6GeI4gii2YCvKLY", //根据自己后台的设置保持一致
                        AppId = MPBasicSetting.AppID    //根据自己后台的设置保持一致

                    };

                    var requestMessageHandler = new MPCustomMessageHandler(Request.InputStream, pm, maxRecordCount);
                    //执行微信处理过程
                    requestMessageHandler.Execute();
                    return Content(requestMessageHandler.ResponseDocument.ToString());
                }
                catch (Exception ex)
                {
                    FileHelper.WriteLog("消息处理错误", ex.Message);
                    return Content("服务器忙...");
                }
                finally
                {

                }

            }
            ViewBag.WeChatTitleName = MPBasicSetting.WeChatTitleName;
            return View();
        }
    }
}