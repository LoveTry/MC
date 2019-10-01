using MC.Code.Data;
using MC.Comm;
using MC.Models.sqllite;
using MCComm;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    /// <summary>
    /// 客户资料管理
    /// </summary>
    [RoutePrefix("api/card")]
    public class CardShareController : BaseCommController
    {
        // GET: CardShare
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            GetgetJsApiTicket();
            return View();
        }

        [HttpPost]
        [Route("save")]
        public string CreateCard(string data)
        {
            try
            {
                JObject job = JObject.Parse(data);
                var item = new CardItem();
                item.desc = CheckData.Check_String(job["desc"]);
                item.headimg = CheckData.Check_String(job["headimg"]);
                item.name = CheckData.Check_String(job["name"]);
                item.openid = CheckData.Check_String(job["openid"]);
                item.phone = CheckData.Check_String(job["phone"]);
                item.wx = CheckData.Check_String(job["wx"]);
                var db = new CardData();
                bool isOk = db.CreateOrUpdate(item);
                if (isOk)
                {
                    string url = Path.Combine(Config.ServerHost, $"cardshare/{item.openid}");
                    string base64 = QrCodeCreater.GetQrCodeImgBase64(url, item.headimg);
                    if (base64.IsNotEmpty())
                    {
                        return OK(new { base64 });
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e);
            }
            return Error("抱歉，生成失败。");
        }
    }
}