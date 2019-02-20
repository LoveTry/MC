using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace MCComm
{
    public class WeixinHelper
    {
        private static string URL = "http://wxmpnet.sunnysoft.com.cn/";

        /// <summary>
        /// 发送意见反馈提醒
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="crtime">提出时间</param>
        /// <param name="feedback">反馈内容</param>
        /// <param name="replyer">回复人</param>
        /// <param name="id">业务主键</param>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        public static bool SendFeedbackWeixin(string title, string crtime, string feedback, string replyer, string id, string openId)
        {
            string first = "您收到意见反馈回复。";
            string keyword1 = title.Length > 20 ? title.Substring(0, 20) + "......" : title;
            string keyword2 = crtime;
            string keyword3 = feedback.Length > 20 ? feedback.Substring(0, 20) + "......" : feedback;
            string keyword4 = replyer;
            string remark = "请点击详情进行查阅！";
            string msg = string.Join("€", first, keyword1, keyword2, keyword3, keyword4, remark);
            string url = URL + "Feedback/FeedbackDetail?ID=" + id + "&openid=" + openId;
            string textType = MessageType.Feedback.GetHashCode().ToString();
            return Get(msg, openId, url, textType);
        }

        /// <summary>
        /// 船舶动态提醒
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="crtime">提出时间</param>
        /// <param name="feedback">反馈内容</param>
        /// <param name="replyer">回复人</param>
        /// <param name="id">业务主键</param>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        public static bool SendNewFeedbackWeixin(string title, string crtime, string feedback, string replyer, string id, string openId)
        {
            string first = "您收到新的意见反馈需要处理。";
            string keyword1 = title.Length > 20 ? title.Substring(0, 20) + "......" : title;
            string keyword2 = crtime;
            string keyword3 = feedback.Length > 20 ? feedback.Substring(0, 20) + "......" : feedback;
            string keyword4 = replyer;
            string remark = "请点击详情进行查阅！";
            string msg = string.Join("€", first, keyword1, keyword2, keyword3, keyword4, remark);
            string url = URL + "Feedback/FeedbackDetail?ID=" + id + "&openid=" + openId;
            string textType = MessageType.Feedback.GetHashCode().ToString();
            return Get(msg, openId, url, textType);
        }

        /// <summary>
        /// 集装箱动态推送
        /// </summary>
        /// <param name="shipname">船名</param>
        /// <param name="voyage">航次</param>
        /// <param name="billno">提单号</param>
        /// <param name="ieflag">进出口标记</param>
        /// <param name="ctnno">集装箱号</param>
        /// <param name="id">业务主键</param>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        public static bool SendShipLiveWeixin(string shipname, string voyage, string billno, string ieflag, string ctnno, string id, string openId)
        {
            string first = "您有新的集装箱动态信息。" + Environment.NewLine +
                "船名：" + shipname + Environment.NewLine + "航次：" + voyage + Environment.NewLine + "进出口：" + ieflag;
            string keyword1 = billno;
            string keyword2 = ctnno;
            string keyword3 = "请点击详情进行查阅！";
            string remark = "更新时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            string msg = string.Join("€", first, keyword1, keyword2, keyword3, remark);
            string url = URL + "Ship/ShipLive?id=" + id + "&userid=" + openId;
            string textType = MessageType.CtnLive.GetHashCode().ToString();
            return Get(msg, openId, url, textType);
        }

        public static string ZipString(string unCompressedString)
        {
            byte[] bytData = System.Text.Encoding.GetEncoding("gb2312").GetBytes(unCompressedString);

            MemoryStream oStream = new MemoryStream();
            DeflateStream zipStream = new DeflateStream(oStream, CompressionMode.Compress);
            zipStream.Write(bytData, 0, bytData.Length);
            zipStream.Flush();
            zipStream.Close();
            byte[] byteResult = oStream.ToArray();

            return System.Convert.ToBase64String(byteResult, 0, byteResult.Length);
        }

        /// <summary>
        /// 发送微信消息GET请求
        /// </summary>
        /// <param name="msg">模板拼接字符串</param>
        /// <param name="openid">openid</param>
        /// <param name="url">点击链接URL</param>
        /// <returns></returns>
        private static bool Get(string msg, string openid, string url, string textType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + "ApiAshx/SendMsg.ashx");
            //加入头信息
            request.Headers.Add("msgFlag", ZipString("海盛"));
            request.Headers.Add("msg", ZipString(msg));
            request.Headers.Add("id", ZipString(openid));
            request.Headers.Add("url", ZipString(url));
            request.Headers.Add("textType", ZipString(textType));

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Timeout = 90000;
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
            string strResult = streamReader.ReadToEnd();
            streamReader.Close();
            streamReceive.Close();
            request.Abort();
            response.Close();
            return strResult.IsEmpty();
        }



    }
}
