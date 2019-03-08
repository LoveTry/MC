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

            Senparc.Weixin.Threads.ThreadUtility.Register();
            Senparc.Weixin.MP.Containers.AccessTokenContainer.Register(MPBasicSetting.AppID, MPBasicSetting.AppSecret, "公众号注册");
            Senparc.Weixin.MP.Containers.JsApiTicketContainer.Register(MPBasicSetting.AppID, MPBasicSetting.AppSecret, "公众号JSSDK注册");
        }
    }
}
