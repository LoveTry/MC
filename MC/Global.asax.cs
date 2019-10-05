using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.CO2NET.Threads;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //初始化信息
            MPBasicSetting.GetBasicSetting();
            //MCComm.DBHelper.GetWebSqlConnection();
            Sunnysoft.DAL.ActiveRecordDBHelper.InitSessionFactory();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var isGLobalDebug = true;//设置全局 Debug 状态
            var senparcSetting = SenparcSetting.BuildFromWebConfig(isGLobalDebug);
            var register = RegisterService.Start(senparcSetting).UseSenparcGlobal();//CO2NET全局注册，必须！

            var isWeixinDebug = true;//设置微信 Debug 状态
            var senparcWeixinSetting = SenparcWeixinSetting.BuildFromWebConfig(isWeixinDebug);
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting);////微信全局注册，必须！

            ThreadUtility.Register();
            Senparc.Weixin.MP.Containers.AccessTokenContainer.RegisterAsync(MPBasicSetting.AppID, MPBasicSetting.AppSecret, "公众号注册");
            Senparc.Weixin.MP.Containers.JsApiTicketContainer.RegisterAsync(MPBasicSetting.AppID, MPBasicSetting.AppSecret, "公众号JSSDK注册");
        }
    }
}
