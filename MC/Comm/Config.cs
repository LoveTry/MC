using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MC.Comm
{
    public class Config
    {
        /// <summary>
        /// 数据存放根目录
        /// </summary>
        public static string DataDir = string.Empty;
        /// <summary>
        /// 账号数据根目录
        /// </summary>
        public static string AccountDataDir = string.Empty;
        /// <summary>
        /// 系统数据目录
        /// </summary>
        public static string SysDataDir = string.Empty;
        /// <summary>
        /// 工具目录
        /// </summary>
        public static string ToolsDir = string.Empty;
        /// <summary>
        /// 系统数据模板目录
        /// </summary>
        public static string SysDataTemplateDir = string.Empty;
        /// <summary>
        /// 服务器根网址
        /// </summary>
        public static string RootURL = string.Empty;
        /// <summary>
        /// 服务器Token
        /// </summary>
        public static string ServerToken = string.Empty;
        /// <summary>
        /// 服务器版本号
        /// </summary>
        public static string ServerVersion = string.Empty;
        /// <summary>
        /// 服务器唯一标识
        /// </summary>
        public static string AppID = string.Empty;
        /// <summary>
        /// 协议授权服务
        /// </summary>
        public static string AuthServer = string.Empty;
        static Config()
        {
            try
            {
                SysDataDir = Path.Combine(ServerInfo.GetRootPath(), "data") + "\\";
                SysDataTemplateDir = Path.Combine(ServerInfo.GetRootPath(), "code") + "\\temp\\";
                DataDir = ConfigurationManager.AppSettings["DataDir"].ToString();
                ToolsDir = SysDataDir + "Tools\\";
                AccountDataDir = Path.Combine(DataDir, "account") + "\\";
                RootURL = ConfigurationManager.AppSettings["wxUrl"].ToString();
                //AppID =ConfigurationManager.AppSettings["AppID"].ToString();
                //AuthServer = ConfigurationManager.AppSettings["AuthServer"].ToString();
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                ServerVersion = fvi.ProductVersion;
            }
            catch (Exception ex)
            {
                LogHelper.Log("Config:" + ex.ToString());
            }
        }
    }
}