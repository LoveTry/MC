using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.Weixin.MP;
using Senparc.Weixin.Entities;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using MCComm;

namespace MC
{
    public class MPMenu
    {
        private static string GetAuthUrl(string url)
        {
            return OAuthApi.GetAuthorizeUrl(MPBasicSetting.AppID, "http://" + MPBasicSetting.wxUrl + "/" + url, "rean", OAuthScope.snsapi_userinfo);
        }
        private static ButtonGroup GetSunnysoftBtnGroup()
        {

            ButtonGroup btnGroup = new ButtonGroup();
            btnGroup.button.Add(new SingleViewButton()
            {
                url = GetAuthUrl("Center/Index"),
                name = MPBasicSetting.WeChatTitleName
            });

            btnGroup.button.Add(new SingleViewButton()
            {
                url = "http://game.cargocargo.cn/",
                name = "微信小游戏"
            });
            #region 注释
            ////第一组：----------------------------------------------
            //SubButton subButtonOne = new SubButton() { name = "航运通+" };

            //subButtonOne.sub_button.Add(new SingleViewButton()
            //{
            //    url = "https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzA3NDgzMjQwOQ==&scene=123#wechat_redirect",
            //    name = "资讯通知"
            //});

            //subButtonOne.sub_button.Add(new SingleViewButton()
            //{
            //    url = "https://e.eqxiu.com/s/vNhfCOfh",
            //    name = "优惠活动"
            //});

            //subButtonOne.sub_button.Add(new SingleClickButton()
            //{
            //    key = "GetUrl",
            //    name = "下载地址"

            //});

            //subButtonOne.sub_button.Add(new SingleViewButton()
            //{
            //    url = "http://tools.sunnysoft.com.cn/Tool/ShipCompany",
            //    name = "常用工具"
            //});

            //subButtonOne.sub_button.Add(new SingleViewButton()
            //{
            //    url = "http://www.sunnysoft.cn/",
            //    name = "盛鸿官网"
            //});


            //btnGroup.button.Add(subButtonOne);


            ////第二组:----------------------------------------------
            //SingleViewButton subButtonTwo = new SingleViewButton() { name = "工作台", url = GetAuthUrl("UserCenter/Index") };



            //btnGroup.button.Add(subButtonTwo);

            ////第三组:我的系统----------------------------------------------
            ////SubButton subButtonThree = new SubButton() { name = "我的" };

            ////subButtonThree.sub_button.Add(new SingleClickButton()
            ////{
            ////    name = "在线客服",
            ////    key = "Help_MenuOne"

            ////});

            ////subButtonThree.sub_button.Add(new SingleViewButton()
            ////{
            ////    url = GetAuthUrl("UserCenter/Index"),
            ////    name = "进入系统"
            ////});

            //var subButtonThree = new SingleClickButton()
            //{
            //    name = "在线客服",
            //    key = "Help_MenuOne"
            //};
            //btnGroup.button.Add(subButtonThree);
            #endregion
            return btnGroup;


        }

        public static string CreateMenuDefault()
        {
            string errormsg = string.Empty;
            try
            {
                ButtonGroup btnGroup = GetSunnysoftBtnGroup();
                WxJsonResult result = CommonApi.CreateMenu(MPBasicSetting.AppID, btnGroup);
                if (result.errcode != ReturnCode.请求成功)
                {
                    errormsg = "错误码:" + result.errcode + ". " + result.errmsg;
                    FileHelper.WriteLog("创建菜单错误", errormsg);
                }
                return errormsg;
            }
            catch (System.Exception ex)
            {
                FileHelper.WriteLog("创建菜单错误", ex.ToString());
                return ex.Message;
            }

        }

        public static GetMenuResult GetMenuDefault()
        {

            return CommonApi.GetMenu(MPBasicSetting.AppID);

        }

        public static void DeleteMenuDefault()
        {

            var result = CommonApi.DeleteMenu(MPBasicSetting.AppID);

        }
    }
}
