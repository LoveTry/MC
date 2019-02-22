using MCComm;
using System;
using System.Data;
using System.Linq;
using System.Web;

namespace MC.Models
{
    public class DBUserInfo
    {
        #region

        public string UserID
        {
            get;
            set;
        }

        public string NickName
        {
            get;
            set;
        }

        public string TrueName
        {
            get;
            set;
        }
        public string openID
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 服务号获取用户信息
        /// </summary>
        /// <param name="wxh"></param>
        /// <returns></returns>
        public static DBUserInfo GetUserDBInfo(string openid)
        {
            //DataTable dt = new DataTable();
            ////dt = NetUsers.GetUserInfoByFWId(wxh);
            //if (dt.Rows.Count > 0)
            //{
            //    DBUserInfo dbUserInfo = new DBUserInfo();
            //    dbUserInfo.TrueName = dt.Rows[0]["TrueName"].ToString();
            //    dbUserInfo.UserID = dt.Rows[0]["UserID"].ToString();
            //    dbUserInfo.NickName = dt.Rows[0]["NickName"].ToString();
            //    dbUserInfo.openID = wxh;

            //    HttpContext.Current.Session.Add(Const.SESSION_USRE_INFO, dbUserInfo);
            //    return dbUserInfo;
            //}
            //return null;
            var info = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(MPBasicSetting.AppID, openid);
            DBUserInfo dbUserInfo = new DBUserInfo();
            dbUserInfo.TrueName = "林春宝";
            dbUserInfo.UserID = GuidHelper.GuidNew().ToString();
            dbUserInfo.NickName = info.nickname;
            dbUserInfo.openID = openid;
            return dbUserInfo;

        }

        /// <summary>
        /// 清空用户Session
        /// </summary>
        public static void CleearUserSession()
        {
            HttpContext.Current.Session.Remove(Const.SESSION_USRE_INFO);
        }


    }
}
