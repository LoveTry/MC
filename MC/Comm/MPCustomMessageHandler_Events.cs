using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace MC
{
    public partial class MPCustomMessageHandler
    {

        public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {

            if (requestMessage.Content == "Help_MenuOne")
            {

                //推送一条客服消息
                try
                {
                    CustomApi.SendText(MPBasicSetting.AppID, WeixinOpenId, "您好！请将您想咨询的问题文字描述或拍照截图，客服人员收到后会与您取得联系。\r\n感谢您使用航运通+ V3.0！");

                }
                catch
                {
                }

                var responseMessageKefu = base.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();

                return responseMessageKefu;

                //var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //strongResponseMessage.Content = @"您好，如需咨询，请回复“客服+咨询内容”。客服转接需要时间，请您稍等。客服在线时间9:00-18:00";
                //return strongResponseMessage;
            }
            else if (requestMessage.Content == "GetUrl")
            {
                var response = base.CreateResponseMessage<ResponseMessageText>();
                response.Content = MPBasicSetting.DownloadV3Url+ "\r\n\r\n\r\n" + "请复制下载地址在电脑端下载安装。";
                return response;
            }

            // 预处理文字或事件类型请求。
            // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
            // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
            // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
            // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
            // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey

            //            if (requestMessage.Content == "Driver_Search")
            //            {
            //                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //                strongResponseMessage.Content = "请输入司机手机号码！支持语音识别.";
            //                return strongResponseMessage;
            //            }
            //            else if (requestMessage.Content == "Bill_Search")
            //            {
            //                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //                strongResponseMessage.Content = "您目前没有账单！";
            //                return strongResponseMessage;
            //            }
            //            else if (requestMessage.Content == "Business_Tracking")
            //            {
            //                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //                strongResponseMessage.Content = "请输入业务号！";
            //                return strongResponseMessage;
            //            }
            //            else if (requestMessage.Content == "Lihuo_Deatil")
            //            {
            //                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //                strongResponseMessage.Content = "请输入理货业务号！";
            //                return strongResponseMessage;
            //            }
            //            else if (requestMessage.Content == "FileUpload_MenuThree")
            //            {
            //                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //                if (CurrentMessageContext.StorageData != null)
            //                {
            //                    strongResponseMessage.Content = "请选择要上传的图片！";
            //                }
            //                else
            //                {
            //                    strongResponseMessage.Content = "您还没有绑定【航运通】微信公众平台，请先进行绑定！" +
            //                        string.Format("<a href='http://" + MPBasicSetting.wxUrl + "/UserBinding.aspx?wxh={0}'>点击进入绑定页面</a>", requestMessage.FromUserName);
            //                }
            //                return strongResponseMessage;
            //            }
            //            else if (requestMessage.Content == "Help_MenuOne")
            //            {
            //                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //                strongResponseMessage.Content = @"如需人工协助，请回复“客服”。（由于人力有限，如
            //需等待请谅解，在线客服服务时间9:00-21:00，或请拔打客服咨询电话：4000363656）";
            //                return strongResponseMessage;
            //            }
            //            else if (requestMessage.Content == "TripGuide")
            //            {
            //                var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
            //                List<Article> lstArt = new List<Article>(){
            //                    new Article
            //                    {
            //                        Title = "出行指南",
            //                        Description = @"您身边的出行小贴示!",
            //                        PicUrl = "http://"+MPBasicSetting.wxUrl+ "/wxImage/cx.jpg",
            //                        Url = ""
            //                    },
            //                        new Article
            //                    {
            //                        Title = "大连出行指南",
            //                        Description = @"大连售票点电话:0411-82820988
            //    我们将竭诚为您服务!",
            //                        PicUrl = "http://"+MPBasicSetting.wxUrl+ "/wxImage/dl.jpg",
            //                        Url = "http://"+MPBasicSetting.wxUrl+ "/dalian.htm"
            //                    },
            //                    new Article
            //                    {
            //                        Title = "烟台出行指南",
            //                        Description = @"烟台售票点电话:0411-82820988
            //    我们将竭诚为您服务!",
            //                        PicUrl = "http://"+MPBasicSetting.wxUrl+ "/wxImage/yt.jpg",
            //                        Url = "http://"+MPBasicSetting.wxUrl+ "/yantai.htm"
            //                    },
            //                    new Article
            //                    {
            //                        Title = "天津出行指南",
            //                        Description = @"天津售票点电话:0411-82820988
            //    我们将竭诚为您服务!",
            //                        PicUrl = "http://"+MPBasicSetting.wxUrl+ "/wxImage/tj.jpg",
            //                        Url = "http://"+MPBasicSetting.wxUrl+ "/tianjin.htm"
            //                    }
            //                };

            //                responseMessage.Articles = lstArt;

            //                return responseMessage;
            //            }
            //            else if (requestMessage.Content == "sunnysoftAbout")
            //            {
            //                var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
            //                List<Article> lstArt = new List<Article>(){
            //                    new Article
            //                    {
            //                        Title =  MPBasicSetting.WeChatTitleName+" 公众平台",
            //                        Description = "专注于供应链,物流管理系统 \n您生意的好助手。",
            //                        PicUrl = "http://"+MPBasicSetting.wxUrl+ "/Images/Welcom1.jpg",
            //                        Url = ""
            //                        //Content=@"<hr><h1>海僡发展 公众平台<h1><br><hr>微信公众号: <b>sunnysoft888</b> <br>软件用盛鸿,生意更兴隆! "
            //                     }
            //                };
            //                responseMessage.Articles = lstArt;

            //                return responseMessage;

            //            }


            return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            #region
            IResponseMessageBase reponseMessage = null;

            //菜单点击，需要跟创建菜单时的Key匹配
            //switch (requestMessage.EventKey)
            //{
            //    case "Help_MenuOne":
            //        {
            //            //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
            //        }
            //        break;
            //    //case "SubClickRoot_Text":
            //    //    {
            //    //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //    //        reponseMessage = strongResponseMessage;
            //    //        strongResponseMessage.Content = "您点击了子菜单按钮。";
            //    //    }
            //    //    break;
            //    //case "SubClickRoot_News":
            //    //    {
            //    //        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
            //    //        reponseMessage = strongResponseMessage;
            //    //        strongResponseMessage.Articles.Add(new Article()
            //    //        {
            //    //            Title = "您点击了子菜单图文按钮",
            //    //            Description = "您点击了子菜单图文按钮，这是一条图文信息。",
            //    //            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg",
            //    //            Url = "http://weixin.senparc.com"
            //    //        });
            //    //    }
            //    //    break;
            //    //case "SubClickRoot_Music":
            //    //    {
            //    //        var strongResponseMessage = CreateResponseMessage<ResponseMessageMusic>();
            //    //        reponseMessage = strongResponseMessage;
            //    //        strongResponseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            //    //    }
            //    //    break;
            //    //case "SubClickRoot_Image":
            //    //    {
            //    //        var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
            //    //        reponseMessage = strongResponseMessage;
            //    //        strongResponseMessage.Image.MediaId = "Mj0WUTZeeG9yuBKhGP7iR5n1xUJO9IpTjGNC4buMuswfEOmk6QSIRb_i98do5nwo";
            //    //    }
            //    //    break;
            //    //case "OAuth"://OAuth授权测试
            //    //    {
            //    //        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
            //    //        strongResponseMessage.Articles.Add(new Article()
            //    //        {
            //    //            Title = "OAuth2.0测试",
            //    //            Description = "点击【查看全文】进入授权页面。\r\n注意：此页面仅供测试（是专门的一个临时测试账号的授权，并非Senparc.Weixin SDK官方账号，所以如果授权后出现错误页面数正常情况），测试号随时可能过期。请将此DEMO部署到您自己的服务器上，并使用自己的appid和secret。",
            //    //            Url = "http://weixin.senparc.com/oauth2",
            //    //            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg"
            //    //        });
            //    //        reponseMessage = strongResponseMessage;
            //    //    }
            //    //    break;
            //    //case "Description":
            //    //    {
            //    //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //    //        strongResponseMessage.Content = "";
            //    //        reponseMessage = strongResponseMessage;
            //    //    }
            //    //    break;
            //}

            return reponseMessage;
            #endregion
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您当前所在位置：\r\n经度：" + requestMessage.Longitude.ToString()
                + "\r\n维度：" + requestMessage.Latitude.ToString()
                + "\r\n精度：" + requestMessage.Precision.ToString();
            return responseMessage; //这里也可以返回null（需要注意写日志时候null的问题）
        }

        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            //通过扫描关注
            return base.OnEvent_ScanRequest(requestMessage);
        }

        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_MassSendJobFinishRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "群发成功记录数:" + requestMessage.SentCount + " 发送结果状态:" + requestMessage.Status;
            return responseMessage;
        }

        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        {
            //FileHelper.WriteLog("LocationSelect", "LocationSelect");

            return base.OnEvent_LocationSelectRequest(requestMessage);
        }

        /// <summary>
        ///Wi-Fi连网成功
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_WifiConnectedRequest(RequestMessageEvent_WifiConnected requestMessage)
        {
            return base.OnEvent_WifiConnectedRequest(requestMessage);
        }



        /// <summary>
        /// 扫码推事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        {
            return base.OnEvent_ScancodePushRequest(requestMessage);
        }

        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            return base.OnEvent_ScancodeWaitmsgRequest(requestMessage);
        }
    }
}
