using MC.Comm;
using MC.Models.sqllite;
using MCComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;

namespace MC.Code.Data
{
    /// <summary>
    /// 微信账号数据
    /// </summary>
    public class WUserData : BaseData
    {
        static ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();
        public WUserData(string openid) : base(openid)
        {
            string sqlitefile = base.GetSqliteFile("wuser.dll", "wuser001.config");
            if (!string.IsNullOrEmpty(sqlitefile))
            {
                this._ConnectionString = this.GetSQLiteConnectionString(sqlitefile);
            }
        }

        /// <summary>
        /// 更新列
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="dicData"></param>
        /// <returns></returns>
        public bool UpdateWUserItemColumns(string uin, Dictionary<string, object> dicData)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                LogHelper.Log("WUserData-UpdateWUserItemColumns:" + "File ERROR");
                return ret;
            }
            if (dicData == null || dicData.Count == 0)
                return ret;
            try
            {
                _LockSlim.EnterWriteLock();
                DapperHelper db = new DapperHelper(this._ConnectionString);
                try
                {
                    db.Update<WUserItem>(dicData, "uin", uin);
                    ret = true;
                }
                catch (Exception ex)
                {
                    LogHelper.Log("WUserData-UpdateWUserItemColumns:" + ex.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ret = false;
                LogHelper.Log("WUserData-UpdateWUserItemColumns:" + ex.ToString());
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
            return ret;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddWUserItem(WUserItem item)
        {
            bool ret = false;
            if (item == null)
                return ret;
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                LogHelper.Log("WUserData-AddWUserItem:" + "File ERROR");
                return ret;
            }
            try
            {
                _LockSlim.EnterWriteLock();
                DapperHelper db = new DapperHelper(this._ConnectionString);
                string sqlSelect = "";
                Dictionary<string, object> parms = new Dictionary<string, object>();
                //parms.Add("uin", item.uin);
                //parms.Add("createtime", item.createtime);
                //parms.Add("alias", item.alias);
                //parms.Add("nickname", item.nickname);
                //parms.Add("username", item.username);
                //parms.Add("city", item.city);
                //parms.Add("province", item.province);
                //parms.Add("pyquanpin", item.pyquanpin);
                //parms.Add("remarkname", item.remarkname);
                //parms.Add("remarkpyquanpin", item.remarkpyquanpin);
                //parms.Add("sex", item.sex);
                //parms.Add("signature", item.signature);
                //parms.Add("headimg", item.headimg);
                //parms.Add("isdel", item.isdel);
                //parms.Add("status", item.status);
                //parms.Add("lastlogintime", item.lastlogintime);
                //parms.Add("devicename", item.devicename);
                //parms.Add("uuid", item.uuid);
                //parms.Add("mac", item.mac);
                try
                {

                    sqlSelect = "SELECT COUNT(uin) FROM WUserItem WHERE uin=@uin";
                    if (db.ExecuteScalar(sqlSelect, parms).ToString().ToInt() > 0)
                    {
                        sqlSelect = "UPDATE WUserItem SET nickname=@nickname,username=@username,status=@status,headimg=@headimg,lastlogintime=@lastlogintime,isdel=@isdel,devicename=@devicename,uuid=@uuid,mac=@mac WHERE uin=@uin";
                    }
                    else
                    {
                        sqlSelect = "INSERT INTO WUserItem(uin,createtime,alias,nickname,username,city,province,pyquanpin,remarkname,remarkpyquanpin,sex,signature,headimg,isdel,status,lastlogintime,devicename,uuid,mac) VALUES(@uin,@createtime,@alias,@nickname,@username,@city,@province,@pyquanpin,@remarkname,@remarkpyquanpin,@sex,@signature,@headimg,@isdel,@status,@lastlogintime,@devicename,@uuid,@mac)";
                    }
                    db.Execute(sqlSelect, parms);
                    ret = true;
                }
                catch (Exception ex)
                {
                    LogHelper.Log("WUserData-AddWUserItem:" + ex.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("WUserData-AddWUserItem:" + ex.ToString());
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
            return ret;
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="visitorid"></param>
        /// <returns></returns>
        public WUserItem GetWUserItem(string uin)
        {
            WUserItem item = null;
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                LogHelper.Log("WUserData-GetWUserItem:" + "File ERROR");
                return item;
            }
            try
            {
                _LockSlim.EnterReadLock();
                string sqlSelect = "SELECT * FROM WUserItem WHERE uin=@uin";
                DapperHelper db = new DapperHelper(this._ConnectionString);
                try
                {
                    item = db.QueryFirstOrDefault<WUserItem>(sqlSelect, new { uin });
                }
                catch (Exception ex)
                {
                    LogHelper.Log("WUserData-GetWUserItem:" + ex.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("WUserData-GetWUserItem:" + ex.ToString());
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
            return item;
        }

        /// <summary>
        /// 重置全部微信号状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ResetWUserStatus(int status = 0)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                LogHelper.Log("WUserData-ResetWUserStatus:" + "File ERROR");
                return ret;
            }
            try
            {
                _LockSlim.EnterWriteLock();
                DapperHelper db = new DapperHelper(this._ConnectionString);
                try
                {
                    string sqlSelect = "UPDATE WUserItem SET status=@status ";
                    ret = db.Execute(sqlSelect, new { status = status }) > 0;
                }
                catch (Exception ex)
                {
                    LogHelper.Log("WUserData-ResetWUserStatus:" + ex.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ret = false;
                LogHelper.Log("WUserData-ResetWUserStatus:" + ex.ToString());
            }
            finally
            {
                _LockSlim.ExitWriteLock();
            }
            return ret;
        }

        /// <summary>
        /// 获取登录微信列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<WUserItem> GetWUserItemList(string where, string order, string name)
        {
            List<WUserItem> list = null;
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                LogHelper.Log("WUserData-GetWUserItemList:" + "File ERROR");
                return list;
            }
            try
            {
                _LockSlim.EnterReadLock();
                string sqlSelect = "SELECT * FROM WUserItem WHERE isdel=0 ";
                DapperHelper db = new DapperHelper(this._ConnectionString);
                try
                {
                    if (!string.IsNullOrEmpty(where))
                    {
                        sqlSelect += " AND (" + where + ")";
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        sqlSelect += " And (nickname like @name OR remarkname like @name)";
                    }
                    if (!string.IsNullOrEmpty(order))
                    {
                        sqlSelect += " ORDER BY " + order;
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        Dictionary<string, object> parms = new Dictionary<string, object>();
                        parms.Add("name", "%" + name + "%");
                        list = db.QueryList<WUserItem>(sqlSelect, parms);
                    }
                    else
                    {
                        list = db.QueryList<WUserItem>(sqlSelect);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log("WUserData-GetWUserItemList:" + ex.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("WUserData-GetWUserItemList:" + ex.ToString());
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
            return list;
        }
        /// <summary>
        /// 获取本地全部有效微信号列表
        /// </summary>
        /// <returns></returns>
        public List<WUserItem> GetWUserList(string order = "lastlogintime ASC")
        {
            List<WUserItem> list = null;
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                LogHelper.Log("WUserData-GetWUserList:" + "File ERROR");
                return list;
            }
            try
            {
                _LockSlim.EnterReadLock();
                string sqlSelect = "SELECT * FROM WUserItem WHERE isdel=0 ";
                DapperHelper db = new DapperHelper(this._ConnectionString);
                try
                {
                    if (!string.IsNullOrEmpty(order))
                    {
                        sqlSelect += " ORDER BY " + order;
                    }
                    list = db.QueryList<WUserItem>(sqlSelect);
                }
                catch (Exception ex)
                {
                    LogHelper.Log("WUserData-GetWUserList:" + ex.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("WUserData-GetWUserList:" + ex.ToString());
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
            return list;
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="currPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<WUserItem> GetWUserItemList(int currPage, int pageSize, ref int recordCount, DateTime begintime, DateTime endtime, string sql, string order)
        {
            List<WUserItem> data = null;
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                LogHelper.Log("WUserData-GetWUserItemList:" + "File ERROR");
                return data;
            }
            try
            {
                _LockSlim.EnterReadLock();
                DapperHelper db = new DapperHelper(this._ConnectionString);
                try
                {
                    string sqlSelect = "SELECT * FROM WUserItem WHERE (createtime >= @begintime AND createtime<=@endtime) ";
                    string sqlCount = "SELECT COUNT(uin) FROM WUserItem WHERE (createtime >= @begintime AND createtime<=@endtime) ";
                    if (!string.IsNullOrEmpty(sql))
                    {
                        sqlSelect += " AND " + sql;
                        sqlCount += " AND " + sql;
                    }
                    if (!string.IsNullOrEmpty(order))
                    {
                        sqlSelect += " ORDER BY " + order;
                    }
                    sqlSelect += " Limit " + pageSize + " Offset " + (currPage - 1) * pageSize;
                    Dictionary<string, object> parms = new Dictionary<string, object>();
                    parms.Add("begintime", begintime);
                    parms.Add("endtime", endtime);

                    data = db.QueryList<WUserItem>(sqlSelect, parms);
                    recordCount = db.ExecuteScalar(sqlCount, parms).ToString().ToInt();
                }
                catch (Exception ex)
                {
                    LogHelper.Log("WUserData-GetWUserItemList:" + ex.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Log("WUserData-GetLeaveMessageItemList:" + ex.ToString());
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
            return data;
        }

        /// <summary>
        /// 获取指定微信的昵称和备注名
        /// </summary>
        /// <param name="ulist">uin集合</param>
        /// <returns></returns>
        public List<WUserItem> GetWUserItemList(List<string> ulist)
        {
            List<WUserItem> list = null;
            if (string.IsNullOrEmpty(this._ConnectionString))
            {
                LogHelper.Log("WUserData-GetWUserItemList:" + "File ERROR");
                return list;
            }
            try
            {
                _LockSlim.EnterReadLock();
                DapperHelper db = new DapperHelper(this._ConnectionString);
                try
                {
                    string sqlSelect = "SELECT uin,nickname,remarkname FROM WUserItem";
                    if (ulist.Count > 0)
                    {
                        sqlSelect += " WHERE uin in (";
                        for (int i = 0; i < ulist.Count; i++)
                        {
                            if (i != 0)
                            {
                                sqlSelect += ",";
                            }
                            sqlSelect += "'" + ulist[i] + "'";
                        }
                        sqlSelect += ")";
                        list = db.QueryList<WUserItem>(sqlSelect);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log("WUserData-GetWUserItemList:" + ex.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("WUserData-GetWUserItemList:" + ex.ToString());
            }
            finally
            {
                _LockSlim.ExitReadLock();
            }
            return list;
        }
    }
}
