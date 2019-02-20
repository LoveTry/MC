using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.GoogleMap;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.Entities.BaiduMap;
using System.Data;
using MCComm;

namespace MC
{
    public class LocationService
    {
        private const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

        public ResponseMessageNews GetResponseMessage(RequestMessageLocation requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
            double lat, lng;
            lat = requestMessage.Location_X;
            lng = requestMessage.Location_Y;

            Convert_GCJ02_To_BD09(ref lat, ref lng);

            var markersList = new List<BaiduMarkers>();
            markersList.Add(new BaiduMarkers()
            {
                Latitude = lat,
                Longitude = lng,
                Color = "red",
                Label = "S",
                Size = BaiduMarkerSize.l,
            });
            //var mapSize = "480x600";
            int scale = requestMessage.Scale;
            scale = 17;
            var mapUrl = BaiduMapHelper.GetBaiduStaticMap(lng, lat, 1,
                scale, markersList, 800, 600);
            //http://map.qq.com/?type=marker&isopeninfowin=1&markertype=1&name=%E4%B8%AD%E5%9B%BD%2C%E8%BE%BD%E5%AE%81%E7%9C%81%2C%E5%A4%A7%E8%BF%9E%E5%B8%82%2C%E4%B8%AD%E5%B1%B1%E5%8C%BA&addr=%E7%8B%AC%E7%AB%8B%E8%A1%9730%E5%8F%B7%E8%91%B5%E8%8B%B1%E8%A1%97&pointy=38.9159&pointx=121.643&coord=38.9159%2C121.643&ref=WeChat
            responseMessage.Articles.Add(new Article()
            {
                //Description = string.Format("您刚才发送了地理位置信息。lng：{0}，lat：{1}，Scale：{2}，标签：{3}",
                //              lng, lat, scale, requestMessage.Label),
                Description = string.Format("您刚才发送了地理位置信息。" + System.Environment.NewLine + "经度：{0}" + System.Environment.NewLine + "纬度：{1}" + System.Environment.NewLine + "{2}",
                              lng, lat, requestMessage.Label),
                PicUrl = mapUrl,
                Title = "定位地点周边地图",
                Url = mapUrl
            });



            //responseMessage.Articles.Add(new Article()
            //{
            //    Title = "微信公众平台SDK 官网链接",
            //    Description = "Senparc.Weixin.MK SDK地址",
            //    PicUrl = "http://weixin.senparc.com/images/logo.jpg",
            //    Url = "http://weixin.senparc.com"
            //});



            return responseMessage;
        }

        public static void Convert_GCJ02_To_BD09(ref double lat, ref double lng)
        {
            double x = lng, y = lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            lng = z * Math.Cos(theta) + 0.0065;
            lat = z * Math.Sin(theta) + 0.006;
        }
    }
}
