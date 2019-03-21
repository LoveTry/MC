using MCComm;
using System;
using System.Data;
using System.Linq;
using System.Web;
using static MC.Models.User;

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
            var info = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(MPBasicSetting.AppID, openid);
            var user = User.FindByProperty("OpenID", info.openid);
            if (user == null)
            {
                //首次关注自动新建用户
                user = new User();
                user.UserID = GuidHelper.GuidNew();
                user.UserName = info.nickname;
                user.OpenID = info.openid;
                user.CrTime = user.LastOnLineTime = DateTime.Now;
                user.Create();
            }
            else
            {
                user.LastOnLineTime= DateTime.Now;
                user.UpdateAndFlush();
            }
            DBUserInfo dbUserInfo = new DBUserInfo();
            dbUserInfo.UserID = user.UserID.ToString();
            dbUserInfo.NickName = user.UserName;
            dbUserInfo.openID = user.OpenID;
            HttpContext.Current.Session.Add(Const.SESSION_USRE_INFO, dbUserInfo);
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
