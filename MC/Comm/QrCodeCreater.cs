using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Comm
{
    public static class QrCodeCreater
    {
        public static string CreateQrCode(string url)
        {
            var qr = QrCodeApi.Create(MPBasicSetting.AppID, 0, (int)CustomerAgent.LIN, QrCode_ActionName.QR_LIMIT_SCENE);
            if (qr.errcode == ReturnCode.请求成功)
            {
                return QrCodeApi.GetShowQrCodeUrl(qr.ticket);
            }
            else
            {
                return null;
            }
        }
    }

    public enum CustomerAgent
    {
        YILICHANG = 1001,
        LIN = 1002,
    }
}