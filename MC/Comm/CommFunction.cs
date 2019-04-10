using System;
using System.Collections.Generic;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using Senparc.Weixin;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using MC.Models;
using MCComm;

namespace MC
{
    public class CommFunction
    {
        private const string ErrorMsg_TimeOut = "该页面已经过期，请退出当前菜单重新进入！";
        /// <summary>
        /// 通过微信发送的code获取数据库用户信息
        /// </summary>
        /// <param name="code"></param>
        public static DBUserInfo GetMPUserInfo(string code, out string openid)
        {
            OAuthAccessTokenResult oauthTokenInfo =
                OAuthApi.GetAccessToken(MPBasicSetting.AppID, MPBasicSetting.AppSecret, code);
            openid = oauthTokenInfo.openid;
            return DBUserInfo.GetUserDBInfo(oauthTokenInfo.openid);
        }

        /// <summary>
        /// 通过微信发送的code获取数据库用户信息
        /// </summary>
        /// <param name="code"></param>
        public static DBUserInfo GetMPUserInfo(string code)
        {
            OAuthAccessTokenResult oauthTokenInfo =
                OAuthApi.GetAccessToken(MPBasicSetting.AppID, MPBasicSetting.AppSecret, code);
            return DBUserInfo.GetUserDBInfo(oauthTokenInfo.openid);
        }

        /// <summary>
        /// 获得状态代码表
        /// </summary>
        /// <param name="codekind">状态类型</param>
        /// <returns></returns>
        //public static List<SelectListItem> GetStatusCode(int codekind)
        //{
        //    //return (from codes in db.t_Status
        //    //        where (codes.BusinessType == codekind && codes.DelFlag == false)
        //    //        orderby codes.OrderSeq
        //    //        select new SelectListItem
        //    //        {
        //    //            //Value =SqlFunctions.StringConvert((double)codes.StatusID),
        //    //            Value = codes.StatusName,
        //    //            Text = codes.StatusName
        //    //        }).ToList();
        //    return new List<SelectListItem>();
        //}

        /// <summary>
        /// 获得基础代码表
        /// </summary>
        /// <param name="codekind">状态类型</param>
        /// <returns></returns>
        public static List<SelectListItem> GetBasicCode(DBUserInfo dbInfo, int codekind, bool isFirstEmpty = true)
        {
            try
            {
                string sql = "SELECT Cname FROM dbo.t_BasicInfoContent WHERE TypeId='{0}' AND DelFlag=0 AND Cname<>'' ORDER BY SequenceOrder ".FormatWith(codekind);
                DataTable dt = new DataTable();

                var query = (from row in dt.AsEnumerable()
                             select new SelectListItem
                             {
                                 Value = row.Field<string>("Cname"),
                                 Text = row.Field<string>("Cname")
                             }).ToList();
                if (isFirstEmpty)
                    query.Insert(0, new SelectListItem() { Value = "", Text = "" });
                return query;
            }
            catch (Exception e)
            {
                FileHelper.WriteLog(e.Message);
                return new List<SelectListItem>();
            }
        }

        /// <summary>
        /// DataTable转换成列数组用于Echarts.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Dictionary<string, string[]> GetArrayByDt(DataTable dt)
        {
            var obj = new Dictionary<string, string[]>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var strArray = new string[dt.Rows.Count];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    strArray[j] = dt.Rows[j][i].ToString();
                }
                obj.Add(dt.Columns[i].ColumnName, strArray);
            }
            return obj;
        }

        public static string HeadImage(string wxfusn)
        {
            try
            {
                if (wxfusn != null && wxfusn != "")
                {
                    Senparc.Weixin.MP.AdvancedAPIs.User.UserInfoJson user = UserApi.Info(MPBasicSetting.AppID, wxfusn);
                    return user.headimgurl;
                }
            }
            catch (Exception e)
            {
                FileHelper.WriteLog("CommFunction_HeadImage", e.Message);
            }
            return "/Images/noImage.png";

        }

        /// <summary>
        /// 从前端请求获取Where字符串
        /// </summary>
        /// <param name="collect"></param>
        /// <returns></returns>
        public static string GetWhere(FormCollection collect)
        {
            string where = "1=1";
            foreach (var key in collect.AllKeys)
            {
                if (key.Equals("X-Requested-With"))
                {
                    continue;
                }
                string value = collect.GetValue(key).AttemptedValue;
                if (value.IsNotEmpty())
                {
                    where += string.Format(" and {0} like '%{1}%'", key, value.Trim());
                }
            }
            return where;
        }

        public static string GetContentType(string fileType)
        {
            switch (fileType)
            {
                case "zip":
                    return "application/x-zip-compressed";
                case "rar":
                    return "application/octet-stream";
                case "txt":
                    return "text/plain";
                case "doc":
                case "docx":
                    return "application/msword";
                case "xls":
                case "xlsx":
                    return "application/vnd.ms-excel";
                case "jpg":
                case "jpeg":
                    return "image/jpeg";
                case "png":
                    return "image/png";
                case "pdf":
                    return "application/pdf";
                case "gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }

        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="tepm">消息模板类</param>
        /// <param name="textType">消息类型</param>
        /// <param name="openId"></param>
        /// <param name="url"></param>
        public static void SendWxTemplateMsg(IMsgTemplate tepm, MessageType textType, string openId, string url = "")
        {
            try
            {
                string tempId = string.Empty;
                switch (textType)
                {
                    case MessageType.Approve://向下一级审核人发送审核通知
                        tempId = MPBasicSetting.流程待审批提醒;
                        break;
                    case MessageType.AprroveResult:
                        tempId = MPBasicSetting.审批结果提醒;
                        break;
                    case MessageType.RemindMsg:
                        tempId = MPBasicSetting.业务交流提醒;
                        break;
                    case MessageType.Feedback:
                        tempId = MPBasicSetting.意见建议反馈通知;
                        break;
                    default:
                        throw new ArgumentNullException("获取模板ID失败，发送微信模板消息需要模板ID");
                }

                var sr = TemplateApi.SendTemplateMessage(MPBasicSetting.AppID, openId, tempId, url, tepm);
                string errmsg = "";
                if (sr.errcode != ReturnCode.请求成功)
                {
                    errmsg = " 错误码:" + sr.errcode + ". " + sr.errmsg + "\r\n";
                    FileHelper.WriteLog(errmsg);
                }
            }
            catch (Exception ex)
            {
                FileHelper.WriteLog(ex.Message);
            }

        }


        /// <summary>
        /// 发送提醒通用模板微信
        /// </summary>
        /// <param name="dbUserInfo"></param>
        /// <param name="receiverIds">一个或多个接受人ID（用","隔开）</param>
        /// <param name="titleName">消息标题名称（业务交流、日报等）</param>
        /// <param name="content">消息内容（自动截取10字）</param>
        /// <param name="url">消息链接</param>
        /// <returns></returns>
        public static bool sendGeneralWxMsg(DBUserInfo dbUserInfo, string receiverIds, string titleName, string content, string url = "")
        {
            try
            {
                string first = "您好。您收到一条{0}消息。".FormatWith(titleName);
                string keyword1 = content.Length > 10 ? content.Substring(0, 9) + "......" : content;
                string keyword2 = string.Format("在{0}发送", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                string remark = "请点击【详情】查看";

                var temp = new 提醒通用模板()
                {
                    first = new TemplateDataItem(first),
                    keyword1 = new TemplateDataItem(keyword1),
                    keyword2 = new TemplateDataItem(keyword2),
                    remark = new TemplateDataItem(remark)
                };
                string[] ids = receiverIds.Split(',');
                foreach (string userid in ids)
                {
                    if (userid.IsNotEmpty())
                    {
                        DataTable dtUser = new DataTable();
                        if (dtUser.Rows.Count == 0)
                            continue;

                        string openId = dtUser.Rows[0]["WxOpenID"].ToString();
                        if (url.IsNotEmpty())
                        {
                            url += "&openid={0}".FormatWith(openId);
                        }
                        SendWxTemplateMsg(temp, MessageType.RemindMsg, openId, url);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                FileHelper.WriteLog(e.Message);
                return false;
            }
        }
    }
}
