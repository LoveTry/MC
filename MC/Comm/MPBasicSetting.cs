using System;
using System.Runtime.CompilerServices;
using MCComm;

namespace MC
{
    public class MPBasicSetting
    {
        public static string UrlExpireTime
        {
            get
            {
                return AppSettingValue();
            }
        }

        public static string NoUseToken
        {
            get
            {
                return AppSettingValue();
            }
        }

        public static string YHTManageUploadImagePath
        {
            get { return AppSettingValue(); }
        }

        public static string 意见建议反馈通知
        {
            get { return AppSettingValue(); }
        }

        public static string DownloadV3Url
        {
            get { return AppSettingValue(); }
        }

        public static string WeChatTitleName = "菁汇";
        public static string MenuCreateFlag = "0";
        public static string FirstCertification = "0";
        public static string EncodingAESKey = "";
        public static string Token = "HYT147SH258SF369"; //与微信公众账号后台的Token设置保持一致，区分大小写。
        public static string AppID = "wx28e1ac68cae5cabb";
        public static string AppSecret = "bd6539fa16305b529acc881b74fe0d51";
        public static string wxUrl = "dc.cargocargo.cn";
        public static string RSA_PrivateKey = "<RSAKeyValue><Modulus>v3xDB04HHPdZmhmO+IY8vOrTrbIKfSJa4FIvaYIUDfGwZhaYOzaygPzn+becBiEt</Modulus><Exponent>AQAB</Exponent><P>5AfAvNpfVdPF2pIldR5sh3j75FgDS/6D</P><Q>1vj77QPLZ9x05CN7N91Oe9Qkt7cRL1KP</Q><DP>NpaojE1Wr0xAPD/qWaxL3O6YlqR/PY0T</DP><DQ>vRyZ2t6MsNiSiCPigLmSEoMErg1A8+Vn</DQ><InverseQ>PW+2F6u/wMc40ovHMjJZ1rU3YIpJlEGH</InverseQ><D>RWm5LpO1dmWf4IG1VxfqOp1xgHyS2suxvxpoW1oCwKoKCVPYSYHMp44p2iS2hbZt</D></RSAKeyValue>";

        /// <summary>
        /// 通知微信号
        /// </summary>
        public static string noticeopid = "";
        public static string SignalRSerUrl = "";
        public static string 审批结果提醒 = "";
        public static string 流程待审批提醒 = "";
        public static string 用户绑定通知 = "";
        public static string 业务交流提醒 = "";
        public static string 集装箱动态通知 = "";
        public static string ErrorMsgExpire = "该页面已经过期，请退出当前菜单重新进入！";
        public static string ErrorMsgNoEq = "微信用户尚未与系统账户匹配，请与管理员联系！";
        public static string ErrorMsgNoRight = "您没有当前应用的权限，请与管理员联系！(是否设置IP白名单)";
        public static void GetBasicSetting()
        {
            try
            {
                FirstCertification = GetConfigvalue("FirstCertification");
                WeChatTitleName = GetConfigvalue("WeChatTitleName");
                MenuCreateFlag = GetConfigvalue("MenuCreateFlag");
                Token = GetConfigvalue("Token");
                wxUrl = GetConfigvalue("wxUrl");
                AppID = GetConfigvalue("AppID");
                AppSecret = GetConfigvalue("AppSecret");
                EncodingAESKey = GetConfigvalue("EncodingAESKey");
                流程待审批提醒 = GetConfigvalue("流程待审批提醒");
                审批结果提醒 = GetConfigvalue("审批结果提醒");
                用户绑定通知 = GetConfigvalue("用户绑定通知");
              
                noticeopid = GetConfigvalue("noticeopid");
               //SignalRSerUrl = NetShare.SerInfo.ServerURL;

            }
            catch (System.Exception ex)
            {
                FileHelper.WriteLog("MPBasicSetting_GetBasicSetting", ex.ToString());
            }
        }

        private static string GetConfigvalue(string name)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[name] == null ? "" :
                    System.Web.Configuration.WebConfigurationManager.AppSettings[name].ToString();
        }

        /// <summary>
        /// CallerMemberName通过属性名自动作为参数进行传达
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string AppSettingValue([CallerMemberName] string key = null)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[key];
        }
    }
}
