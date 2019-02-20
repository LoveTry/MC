/*----------------------------------------------------------------
    Copyright (C) 2015 Sunnysoft
    
    文件名：FileHelper.cs
    文件功能描述：根据完整文件路径获取FileStream
    
    
    创建标识：Sunnysoft - 20150211
    
    修改标识：Sunnysoft - 20150303
    修改描述：整理接口
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace MCComm
{
    public class FileHelper
    {

        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetFileStream(string fileName)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open);
            }
            return fileStream;
        }

        public static void WriteLog(string typename, string content)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("/FileLog/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(path + DateTime.Now.ToString("yyyyMMdd_HH") + ".txt", "\r\n" + DateTime.Now.ToString("hh:mm") + " " + typename + "\t" + content, Encoding.UTF8);
        }

        /// <summary>
        ///自动输出log  类名_方法名(只适用public方法)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLog(string message)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("/FileLog/");
            string typename = (new StackTrace()).GetFrame(1).GetMethod().ReflectedType.Name + "_" + (new StackTrace()).GetFrame(1).GetMethod().Name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(path + DateTime.Now.ToString("yyyyMMdd_HH") + ".txt", "\r\n" + DateTime.Now.ToString("hh:mm") + " " + typename + "\t" + message, Encoding.UTF8);
        }

        /// <summary>
        ///自动输出log  类名_方法名(只适用public方法)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteWinLog(string message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/FileLog/";
            string typename = (new StackTrace()).GetFrame(1).GetMethod().ReflectedType.Name + "_" + (new StackTrace()).GetFrame(1).GetMethod().Name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(path + DateTime.Now.ToString("yyyyMMdd_HH") + ".txt", "\r\n" + DateTime.Now.ToString("hh:mm") + " " + typename + "\t" + message, Encoding.UTF8);
        }

        public static void WriteWinLog(string funcName, string content)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/FileLog/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(path + DateTime.Now.ToString("yyyyMMdd_HH") + ".txt", "\r\n" + DateTime.Now.ToString("hh:mm") + " " + funcName + "\t" + content, Encoding.UTF8);
        }

        /// <summary>
        /// 解析DataTable到Log文件中
        /// </summary>
        /// <param name="dt"></param>
        public static void WriteDtInfo(DataTable dt)
        {
            if (dt == null)
            {
                WriteDateTableFile("DataTable", "NULL");
                return;
            }
            WriteDateTableFile("DataTable", "===========================");
            WriteDateTableFile("DataTableCount", dt.Rows.Count.ToString());
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append(dt.Columns[i].Caption + " ");
            }
            WriteDateTableFile("DataTableColumns", sb.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                sb = new StringBuilder();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(dr[i] == DBNull.Value ? "NULL" : dr[i].ToString() + " ");
                }
                WriteDateTableFile("DataTableRows", sb.ToString());
            }
            WriteDateTableFile("DataTable", "===========================");
        }

        private static void WriteDateTableFile(string name, string content)
        {
            string Path = System.Web.HttpContext.Current.Server.MapPath("/");
            File.AppendAllText(Path + "/FileLog/" + DateTime.Now.ToString("yyyyMMdd_HH") + ".txt", "\r\n" + DateTime.Now.ToString("hh:mm") + " " + name + "\t" + content, Encoding.UTF8);
        }

        public static void WriteWinDtInfo(DataTable dt)
        {
            if (dt == null)
            {
                WriteWinDateTableFile("DataTable", "NULL");
                return;
            }
            WriteWinDateTableFile("DataTable", "===========================");
            WriteWinDateTableFile("DataTableCount", dt.Rows.Count.ToString());
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append(dt.Columns[i].Caption + " ");
            }
            WriteWinDateTableFile("DataTableColumns", sb.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                sb = new StringBuilder();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(dr[i] == DBNull.Value ? "NULL" : dr[i].ToString() + " ");
                }
                WriteWinDateTableFile("DataTableRows", sb.ToString());
            }
            WriteWinDateTableFile("DataTable", "===========================");
        }

        private static void WriteWinDateTableFile(string name, string content)
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory;
            File.AppendAllText(Path + "/FileLog/" + DateTime.Now.ToString("yyyyMMdd_HH") + ".txt", "\r\n" + DateTime.Now.ToString("hh:mm") + " " + name + "\t" + content, Encoding.UTF8);
        }


        /// <summary>  
        /// 执行DOS命令，返回DOS命令的输出  
        /// </summary>  
        /// <param name="dosCommand">dos命令</param>  
        /// <param name="milliseconds">等待命令执行的时间（单位：毫秒），  
        /// 如果设定为0，则无限等待</param>  
        /// <returns>返回DOS命令的输出</returns>  
        public static string Execute(string command, int seconds)
        {
            string output = ""; //输出字符串  
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令  
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动 
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程  
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束  
                        }
                        else
                        {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);//捕获异常，输出异常信息
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }
    }
}
