using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.MessageHandlers;
using System.IO;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Entities;
using System.Data;
using System.Threading.Tasks;
using Senparc.Weixin.MP.AdvancedAPIs;
using MCComm;
using MC.Comm;

namespace MC
{
    public partial class MPCustomMessageHandler : MessageHandler<MPCustomMessageContext>
    {
        public MPCustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            MessageHandler<MPCustomMessageContext>.GlobalWeixinContext.ExpireMinutes = 30;
            //WeixinContext.ExpireMinutes = 4;
        }

        public override void OnExecuting()
        {
            //BLL.DBUserInfo dbUserInfo = BLL.DBUserInfo.GetUserDBInfo(MPCustomMessageContext.UserName);
            //if (dbUserInfo != null)
            //{
            //    MPCustomMessageContext.StorageData = dbUserInfo;
            //}
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            //MPCustomMessageContext.StorageData = ((int)MPCustomMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            switch (requestMessage.Content)
            {

                case "创建菜单":
                    responseMessage.Content = "菜单更新命令已发出！";
                    if (MPBasicSetting.MenuCreateFlag.Equals("1"))
                    {
                        responseMessage.Content = MPMenu.CreateMenuDefault() == "" ? "成功创建菜单!" : "创建菜单失败";
                    }
                    break;

                default:

                    try
                    {
                        CustomApi.SendText(MPBasicSetting.AppID, WeixinOpenId, "您好！请将您想咨询的问题文字描述或拍照截图，客服人员收到后会与您取得联系。\r\n感谢您使用航运通+ V3.0！");

                    }
                    catch
                    {
                    }

                    var responseMessageKefu = base.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();

                    return responseMessageKefu;

            }
            return responseMessage;
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            return null;
            //var responseMessage = CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "暂时不能识别语音！";
            //return responseMessage;

            //var responseMessageKefu = base.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
            //return responseMessageKefu;

        }


        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            #region
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            //responseMessage.Content = GetWelcomeInfo();
            //return responseMessage;
            //BLL.BUser Subuser = new BLL.BUser();
            //UserInfoJson uij = Subuser.GetUserInfo(requestMessage.FromUserName);
            #endregion
            MCComm.FileHelper.WriteLog("OnEvent_SubscribeRequest", requestMessage.EventKey);
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);

            responseMessage.Articles.Add(new Article
            {
                Title = "欢迎关注{0}公众平台".FormatWith(MPBasicSetting.WeChatTitleName),
                Description = "轻松赚钱 改变生活",
                PicUrl = "http://" + MPBasicSetting.wxUrl + "/Images/welcome.jpg"
                //Url = "http://" + MPBasicSetting.wxUrl + "/Account/UserBing?openid=" + responseMessage.ToUserName + "&IsSkip=1"
            });
            //responseMessage.Articles.Add(new Article
            //{
            //    Title = "点击此处获取航运通+V3.0下载地址",
            //    Description = "www.sunnysoft.com.cn/HangyuntongV3.aspx",
            //    PicUrl = "http://" + MPBasicSetting.wxUrl + "/Images/downloadURL.png",
            //    Url = "http://" + MPBasicSetting.wxUrl + "/Account/DownURL"
            //});

            return responseMessage;
        }


        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            //已关注扫描带参数的二维码
            MCComm.FileHelper.WriteLog("OnEvent_ScanRequest", requestMessage.EventKey);
            if (requestMessage.EventKey.IsNotEmpty())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                switch (requestMessage.EventKey.ToInt())
                {
                    case (int)CustomerAgent.YILICHANG:
                        responseMessage.Content = "亿利昶专供一次性餐饮用品，服务至上。详情请点击<a href='www.dc.cargocargo.cn/ad/1001'>www.dc.cargocargo.cn/ad/ad/1001</a>";
                        return responseMessage;
                    case (int)CustomerAgent.LIN:
                        responseMessage.Content = "艺利广告，专注新媒体。详情请点击<a href='www.dc.cargocargo.cn/ad/1001'>www.dc.cargocargo.cn/ad/ad/1002</a>";
                        return responseMessage;
                    case 0:
                        return base.OnEvent_ScanRequest(requestMessage);
                    default:
                        return base.OnEvent_ScanRequest(requestMessage);
                }
            }
            return base.OnEvent_ScanRequest(requestMessage);
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            return null;

            //var responseMessageKefu = base.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
            //return responseMessageKefu;

            //StringBuilder sb = new StringBuilder();
            //var responseMessage = CreateResponseMessage<ResponseMessageText>();


            //// string uploadSuccess = BLL.BMedia.GetDefault(requestMessage.MediaId, requestMessage.FromUserName);
            //return responseMessage;
        }



        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            return null;
            //var responseMessageKefu = base.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
            //return responseMessageKefu;

            //var responseMessage = CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "您发送了一条视频信息，ID：" + requestMessage.MediaId;
            //return responseMessage;
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            return null;
            //            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            //            responseMessage.Content = string.Format(@"您发送了一条连接信息：
            //Title：{0}
            //Description:{1}
            //Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            //            return responseMessage;
        }


        public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        {
            return base.OnShortVideoRequest(requestMessage);
        }


        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs


            return eventResponseMessage;
        }

        /// <summary>
        /// 默认消息事件，不特殊处理的话，就会都到这里来。
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */
            MCComm.FileHelper.WriteLog("DefaultResponseMessage", ((int)requestMessage.MsgType).ToString());

            var responseMessage = this.CreateResponseMessage<ResponseMessageNoResponse>();
            return responseMessage;
        }
    }
}
