using MC.Code.Data;
using MC.Comm;
using MC.Models.sqllite;
using MCComm;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;
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
    public class CardShareController : BaseCommController
    {
        // GET: CardShare
        public ActionResult get(string openid, long id)
        {
            if (openid.IsEmpty() || id == 0)
            {
                return Content("参数不正确");
            }
            ViewBag.openid = openid;
            ViewBag.id = id;
            return View("index");
        }

        public string OnReady(string openid, long id)
        {
            if (openid.IsEmpty() || id == 0)
            {
                return Error("参数不正确");
            }
            var cardData = new CardData(openid);
            var item = cardData.GetCardItem(id);
            var list = cardData.GetCardSubItemList(id);
            return OK(new { item, list });
        }

        public ActionResult Create()
        {
            GetgetJsApiTicket();
            return View();
        }

        [HttpPost]
        public string CreateCard(string data)
        {
            try
            {
                JObject job = JObject.Parse(data);
                var item = new CardItem();
                item.desc = CheckData.Check_String(job["desc"]);
                item.name = CheckData.Check_String(job["name"]);
                item.openid = OpenId;
                item.phone = CheckData.Check_String(job["phone"]);
                item.wx = CheckData.Check_String(job["wx"]);
                var imgurl = CheckData.Check_String(job["headimg"]);
                var path = Path.Combine(Config.AccountDataDir, this.OpenId + "\\files\\avatars\\");
                string filename = GetFile(imgurl, path);
                item.headimg = filename;
                var db = new CardData(OpenId);
                long id = db.CreateOrUpdate(item);
                if (id > 0)
                {
                    foreach (var img in job["resimg"])
                    {
                        var sub = new CardSubItem();
                        sub.cardid = id;
                        var imgPath = Path.Combine(Config.AccountDataDir, this.OpenId + "\\files\\img\\");
                        string filename2 = GetFile(CheckData.Check_String(img), imgPath);
                        sub.path = filename2;
                        db.CreateOrUpdate(sub);
                    }
                    string url = "http://" + Config.RootURL + $"/cardshare/get?openid={item.openid}&id={id}";
                    string avatarsPath = Path.Combine(Config.AccountDataDir, this.OpenId + "\\files\\avatars\\" + filename);
                    string base64 = QrCodeCreater.GetQrCodeImgBase64(url, avatarsPath);
                    if (base64.IsNotEmpty())
                    {
                        return OK(base64);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e.Message);
            }
            return Error("抱歉，生成失败。");
        }


        private string GetFile(string serverid, string path)
        {
            try
            {
                string name = Guid.NewGuid().ToString("n").Substring(0, 8) + ".jpg";

                using (MemoryStream ms = new MemoryStream())
                {
                    MediaApi.Get(MPBasicSetting.AppID, serverid, ms);
                    SaveFile(path, name, ms.ToArray());
                }
                return name;
            }
            catch (Exception e)
            {
                LogHelper.Log(e.Message);
            }
            return string.Empty;
        }
    }
}