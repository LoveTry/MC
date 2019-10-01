using MCComm;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace MC.Comm
{
    public static class QrCodeCreater
    {
        public static string WXCreateQrCode(string url)
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

        public static string GetQrCodeImgBase64(string url, string headimg = "")
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            System.Drawing.Image image = qrCodeEncoder.Encode(url);
            if (image != null)
            {
                if (headimg.IsEmpty())
                {
                    return CImageLibrary.ToBase64(image);
                }
                else
                {
                    var img = CombinImage(image, headimg);
                    if (image != null)
                    {
                        return CImageLibrary.ToBase64(image);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            else
                return string.Empty;
        }

        /// <summary>    
        /// 调用此函数后使此两种图片合并，类似相册，有个    
        /// 背景图，中间贴自己的目标图片    
        /// </summary>    
        /// <param name="imgBack">粘贴的二维码</param>    
        /// <param name="destImg">粘贴的头像</param>    
        private static Image CombinImage(Image qrImg, string destImg)
        {
            try
            {
                Image img = Image.FromFile(destImg);        //照片图片      
                if (img.Height != 65 || img.Width != 65)
                {
                    img = KiResizeImage(img, 65, 65, 0);
                }
                Graphics g = Graphics.FromImage(qrImg);

                g.DrawImage(qrImg, 0, 0, qrImg.Width, qrImg.Height);      //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

                //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

                //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    

                g.DrawImage(img, qrImg.Width / 2 - img.Width / 2, qrImg.Width / 2 - img.Width / 2, img.Width, img.Height);
                GC.Collect();
                return qrImg;
            }
            catch (Exception ex)
            {
                LogHelper.Log("QrCodeCreater-CombinImage:" + ex.ToString());
            }
            return null;
        }

        /// <summary>    
        /// Resize图片    
        /// </summary>    
        /// <param name="bmp">原始Bitmap</param>    
        /// <param name="newW">新的宽度</param>    
        /// <param name="newH">新的高度</param>    
        /// <param name="Mode">保留着，暂时未用</param>    
        /// <returns>处理以后的图片</returns>    
        private static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception ex)
            {
                LogHelper.Log("QrCodeCreater-CombinImage:" + ex.ToString());
            }
            return null;
        }
    }

    public enum CustomerAgent
    {
        YILICHANG = 1001,
        LIN = 1002,
    }
}