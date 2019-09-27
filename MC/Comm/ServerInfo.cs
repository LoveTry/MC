using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MC.Comm
{
    /// <summary>
    /// WEB信息
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// 取得网站的根目录的URL
        /// </summary>
        /// <returns></returns>
        public static string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;
                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在   Web   站点   
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下   
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        }
        /// <summary>
        /// 取得网站的根目录的URL
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        public static string GetRootURI(HttpRequest Req)
        {
            string AppPath = "";
            if (Req != null)
            {
                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在   Web   站点   
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下   
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        }
        /// <summary>
        /// 取得网站根目录的物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }

        public static string RootPath()
        {
            return RootPath("/");
        }
        public static string RootPath(string filePath)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string separator = Path.DirectorySeparatorChar.ToString();
            rootPath = rootPath.Replace("/", separator);
            if (filePath != null)
            {
                filePath = filePath.Replace("/", separator);
                if (((filePath.Length > 0) && filePath.StartsWith(separator)) && rootPath.EndsWith(separator))
                {
                    rootPath = rootPath + filePath.Substring(1);
                }
                else
                {
                    rootPath = rootPath + filePath;
                }
            }
            return rootPath;
        }

        public static string PhysicalPath(string path)
        {
            return (RootPath().TrimEnd(new char[] { Path.DirectorySeparatorChar })
                  + Path.DirectorySeparatorChar.ToString() + path.TrimStart(new char[] { Path.DirectorySeparatorChar }));
        }

        public static string MapPath(string path)
        {
            HttpContext context = HttpContext.Current;
            if (context != null && context.Server != null)
            {
                return context.Server.MapPath(path);
            }
            return PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
        }





    }
}