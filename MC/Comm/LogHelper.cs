using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MC.Comm
{
    public class LogHelper
    {
        private static object checkobj = new object();

        /// <summary>
        /// 测试路径配置是否正确
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool CheckDirValid(string dir)
        {
            try
            {
                lock (checkobj)
                {
                    string txt = DateTime.Now.Ticks.ToString();
                    string testfile = Path.Combine(dir, "test.txt");
                    StreamWriter writer = new StreamWriter(testfile, false);
                    writer.WriteLine(txt);
                    writer.Flush();
                    writer.Close();

                    StreamReader s = File.OpenText(testfile);
                    string read = s.ReadLine();
                    s.Close();

                    File.Delete(testfile);

                    return string.Compare(txt, read) == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private static object lockobj = new object();
        public static void Log(string logMessage, string filename)
        {
            try
            {
                lock (lockobj)
                {
                    StreamWriter w;
                    string path1 = Path.Combine(Config.SysDataDir, "logs");
                    if (!System.IO.Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(path1);
                    }
                    path1 = Path.Combine(path1, filename);
                    if (!File.Exists(path1))
                    {
                        w = File.CreateText(path1);
                    }
                    else
                    {
                        w = File.AppendText(path1);
                    }
                    w.Write("\r\nLog Entry : ");
                    w.WriteLine("{1} {0}", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());
                    w.WriteLine("{0}", logMessage);
                    w.WriteLine("-------------------------------");
                    w.Flush();
                    w.Close();
                }
            }
            catch
            { }
        }
        private static string LogFileName
        {
            get
            {
                string n = DateTime.Now.ToString("yyMMdd");
                return ("log" + n + ".txt");
            }
        }
        public static void Log(string s)
        {
            Log(s, LogFileName);
        }
        public static void Log(object o)
        {
            Log(o.ToString(), LogFileName);
        }
        /// <summary>
        /// 异常记录，格式：异常文字信息\r\n异常堆栈信息
        /// </summary>
        /// <param name="ex">捕捉到的异常对象</param>
        public static void Log(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            Log(ex.Message + "\r\n" + ex.StackTrace, LogFileName);
        }
        public static void Log(string s, object[] oo)
        {
            foreach (object o in oo)
            {
                s += o + ",";
            }
            Log(s, LogFileName);
        }
    }
}