using MC.Comm;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;

namespace MC.Code.Data
{
    public abstract class BaseData
    {
        public long _Accountid = 0;
        public string _ConnectionString = string.Empty;
        public BaseData() { }
        public BaseData(long accountid)
        {
            this._Accountid = accountid;
        }
        /// <summary>
        /// 获取数据源文件
        /// </summary>
        /// <param name="template">模板文件</param>
        /// <param name="filename">目标数据源文件</param>
        /// <returns></returns>
        public virtual string GetSqliteFile(string template, string filename)
        {
            try
            {
                string filepath = Path.Combine(Config.AccountDataDir, this._Accountid + "\\");
                if (!System.IO.Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath += filename;
                if (!File.Exists(filepath))
                {
                    System.IO.File.Copy(Config.SysDataTemplateDir + template, filepath);
                    return filepath;
                }
                if (File.Exists(filepath))
                {
                    if (File.GetAttributes(filepath) != FileAttributes.Normal) File.SetAttributes(filepath, FileAttributes.Normal);
                    return filepath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("BaseData-GetSqliteFile:" + ex.ToString());
                return null;
            }
        }

        public virtual string GetSQLiteConnectionString(string sqlitePath)
        {
            string retStr = "data source=" + sqlitePath + ";password={0};";
            retStr = string.Format(retStr, "m!d@k#");
            return retStr;
        }
    }
}