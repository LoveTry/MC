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

        public static string 订单生成通知
        {
            get { return AppSettingValue(); }
        }

        public static string 推荐成交通知
        {
            get { return AppSettingValue(); }
        }

        public static string 审核结果通知
        {
            get { return AppSettingValue(); }
        }

        public static string DownloadV3Url
        {
            get { return AppSettingValue(); }
        }

        public static string WeChatTitleName = "联云微传媒";
        public static string MenuCreateFlag = "0";
        public static string FirstCertification = "0";
        public static string EncodingAESKey = "";
        public static string Token = "L147C258B369"; //与微信公众账号后台的Token设置保持一致，区分大小写。
        public static string AppID = "wx1c5011637f84a922";
        public static string AppSecret = "fd5173e3273f6f6c28dd562b659c49b3";
        public static string wxUrl = "mc.cargocargo.cn";
        public static string RSA_PrivateKey = "<RSAKeyValue><Modulus>v3xDB04HHPdZmhmO+IY8vOrTrbIKfSJa4FIvaYIUDfGwZhaYOzaygPzn+becBiEt</Modulus><Exponent>AQAB</Exponent><P>5AfAvNpfVdPF2pIldR5sh3j75FgDS/6D</P><Q>1vj77QPLZ9x05CN7N91Oe9Qkt7cRL1KP</Q><DP>NpaojE1Wr0xAPD/qWaxL3O6YlqR/PY0T</DP><DQ>vRyZ2t6MsNiSiCPigLmSEoMErg1A8+Vn</DQ><InverseQ>PW+2F6u/wMc40ovHMjJZ1rU3YIpJlEGH</InverseQ><D>RWm5LpO1dmWf4IG1VxfqOp1xgHyS2suxvxpoW1oCwKoKCVPYSYHMp44p2iS2hbZt</D></RSAKeyValue>";

        /// <summary>
        /// 通知微信号
        /// </summary>
        public static string noticeopid = "";
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
                noticeopid = GetConfigvalue("noticeopid");

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
