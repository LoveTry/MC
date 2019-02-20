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
            return OAuthApi.GetAuthorizeUrl(MPBasicSetting.AppID, "http://" + MPBasicSetting.wxUrl + "/" + url, "sunnysoft", OAuthScope.snsapi_base);
        }
        private static ButtonGroup GetSunnysoftBtnGroup()
        {
            #region

            ButtonGroup btnGroup = new ButtonGroup();

            //第一组：----------------------------------------------
            SubButton subButtonOne = new SubButton() { name = "航运通+" };

            subButtonOne.sub_button.Add(new SingleViewButton()
            {
                url = "https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzA3NDgzMjQwOQ==&scene=123#wechat_redirect",
                name = "资讯通知"
            });

            subButtonOne.sub_button.Add(new SingleViewButton()
            {
                url = "https://e.eqxiu.com/s/vNhfCOfh",
                name = "优惠活动"
            });

            subButtonOne.sub_button.Add(new SingleClickButton()
            {
                key = "GetUrl",
                name = "下载地址"

            });

            subButtonOne.sub_button.Add(new SingleViewButton()
            {
                url = "http://tools.sunnysoft.com.cn/Tool/ShipCompany",
                name = "常用工具"
            });

            subButtonOne.sub_button.Add(new SingleViewButton()
            {
                url = "http://www.sunnysoft.cn/",
                name = "盛鸿官网"
            });


            #region old
            //SingleViewButton btnProduction = new SingleViewButton();
            //btnProduction.url = "http://www.sunnysoft.cn/";
            //btnProduction.name = "产品与案例";
            //subButtonOne.sub_button.Add(btnProduction);
            #endregion


            btnGroup.button.Add(subButtonOne);


            //第二组:----------------------------------------------
            SingleViewButton subButtonTwo = new SingleViewButton() { name = "工作台", url = GetAuthUrl("UserCenter/Index") };


            #region old
            //SingleViewButton btnApplyUse = new SingleViewButton();
            //btnApplyUse.url = GetAuthUrl("Hangyuntong/ApplyUse");
            //btnApplyUse.name = "客服";

            //subButtonTwo.sub_button.Add(btnApplyUse);


            //SingleViewButton btnApplyUse = new SingleViewButton();
            //btnApplyUse.url = GetAuthUrl("Hangyuntong/ApplyUse");
            //btnApplyUse.name = "下载试用";
            //subButtonTwo.sub_button.Add(btnApplyUse);


            //SingleViewButton btnDiscounts = new SingleViewButton();
            //btnDiscounts.url = GetAuthUrl("Hangyuntong/Discounts");
            //btnDiscounts.name = "优惠活动";
            //subButtonTwo.sub_button.Add(btnDiscounts);


            //SingleViewButton btnMore = new SingleViewButton();
            //btnMore.url = GetAuthUrl("Hangyuntong/More");
            //btnMore.name = "更多产品";
            //subButtonTwo.sub_button.Add(btnMore);
            #endregion



            btnGroup.button.Add(subButtonTwo);

            //第三组:我的系统----------------------------------------------
            //SubButton subButtonThree = new SubButton() { name = "我的" };

            //subButtonThree.sub_button.Add(new SingleClickButton()
            //{
            //    name = "在线客服",
            //    key = "Help_MenuOne"

            //});

            //subButtonThree.sub_button.Add(new SingleViewButton()
            //{
            //    url = GetAuthUrl("UserCenter/Index"),
            //    name = "进入系统"
            //});

            #region old
            //SingleViewButton btnMobilePlatform = new SingleViewButton();
            //btnMobilePlatform.url = GetAuthUrl("UserCenter/Index");
            //btnMobilePlatform.name = "进入系统";
            //subButtonThree.sub_button.Add(btnMobilePlatform);



            //SingleViewButton btnUserbing = new SingleViewButton();
            //btnUserbing.url = GetAuthUrl("Account/UserBing");
            //btnUserbing.name = "用户绑定";
            //subButtonThree.sub_button.Add(btnUserbing);

            //SingleViewButton btnViewcommunication = new SingleViewButton();
            //btnViewcommunication.url = GetAuthUrl("BusinessSession/SessionList");
            //btnViewcommunication.name = "业务交流";
            //subButtonThree.sub_button.Add(btnViewcommunication);


            //SingleViewButton btnViewApprove = new SingleViewButton();
            //btnViewApprove.url = GetAuthUrl("Approve/ApproveList");
            //btnViewApprove.name = "我要审批";
            //subButtonThree.sub_button.Add(btnViewApprove);
            #endregion

            var subButtonThree = new SingleClickButton()
            {
                name = "在线客服",
                key = "Help_MenuOne"
            };
            btnGroup.button.Add(subButtonThree);

            #region
            ////第一组----------------------------------------------

            //SubButton subButtonOne = new SubButton() { name = "会员管理" };

            //SingleViewButton btnUserBinding = new SingleViewButton();
            //btnUserBinding.url = GetAuthUrl("Account/UserBing");
            //btnUserBinding.name = "员工绑定";
            //subButtonOne.sub_button.Add(btnUserBinding);

            //SingleViewButton btnLinkManBinding = new SingleViewButton();
            //btnLinkManBinding.url = GetAuthUrl("Account/LinkMan");
            //btnLinkManBinding.name = "客户绑定";
            //subButtonOne.sub_button.Add(btnLinkManBinding);

            //btnGroup.button.Add(subButtonOne);

            ////第二组----------------------------------------------
            //SingleViewButton btnViewApprove = new SingleViewButton();
            //btnViewApprove.url = GetAuthUrl("Approve/ApproveList");
            //btnViewApprove.name = "我要审批";
            //btnGroup.button.Add(btnViewApprove);

            ////SingleLocationSelectButton btnVehicleLocation2 = new SingleLocationSelectButton();
            ////btnVehicleLocation2.key = "upLocation";
            ////btnVehicleLocation2.name = "自助定位";
            ////btnVehicleLocation2.type = "location_select";

            ////subButtonTwo.sub_button.Add(btnVehicleLocation2);

            ////btnGroup.button.Add(subButtonTwo);

            ////第二组----------------------------------------------
            //SubButton subButtonTwo = new SubButton() { name = "现场管理" };

            //SingleViewButton btnWorkSite = new SingleViewButton();

            //btnWorkSite.url = GetAuthUrl("WorkSite/");
            //btnWorkSite.name = "现场作业";
            //subButtonTwo.sub_button.Add(btnWorkSite);

            //SingleLocationSelectButton btnVehicleLocation2 = new SingleLocationSelectButton();
            //btnVehicleLocation2.key = "upLocation";
            //btnVehicleLocation2.name = "自助定位";
            //btnVehicleLocation2.type = "location_select";

            //subButtonTwo.sub_button.Add(btnVehicleLocation2);

            //btnGroup.button.Add(subButtonTwo);

            ////第三组----------------------------------------------
            //SubButton subButtonThree = new SubButton() { name = "自助查询" };

            //SingleViewButton btnSelect = new SingleViewButton();
            //btnSelect.url = GetAuthUrl("Search/BusinessQuery");
            //btnSelect.name = "业务阶段查询";
            //subButtonThree.sub_button.Add(btnSelect);

            //SingleViewButton btnSelect2 = new SingleViewButton();
            //btnSelect2.url = GetAuthUrl("Search/ContainerQuery");
            //btnSelect2.name = "集装箱查询";
            //subButtonThree.sub_button.Add(btnSelect2);

            //btnGroup.button.Add(subButtonThree);
            #endregion

            return btnGroup;

            #endregion
        }

        public static string CreateMenuDefault()
        {
            string errormsg = string.Empty;
            try
            {
                ButtonGroup btnGroup = new ButtonGroup();
                switch (MPBasicSetting.MenuType)
                {
                    case "sunnysoft":
                    case "hyt":
                        btnGroup = GetSunnysoftBtnGroup();
                        break;
                }
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
