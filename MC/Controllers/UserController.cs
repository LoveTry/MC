using MC.Models;
using MCComm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace MC.Controllers
{
    public class UserController : CommonController
    {
        // GET: User
        public ActionResult Update()
        {
            ViewBag.SexList = new List<SelectListItem>() {
                new SelectListItem() {Text="",Value="" },
                new SelectListItem() { Text="男", Value="男" },
                new SelectListItem() { Text="女", Value="女" },
            };
            var user = Models.User.TryFind(DBUserInfo.UserID.ToGuid());
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(User model)
        {
            if (TryValidateModel(model))
            {
                model.Update();
                DBUserInfo.CleearUserSession();
                DBUserInfo.GetUserDBInfo(model.OpenID);
            }
           
            return Json(new { url = "/Center/Index/4" });
        }

        public ActionResult Index()
        {
            Qrcode();
            return View();
        }

        public void Qrcode()
        {
            string url = "http://mc.cargocargo.cn/cardshare";
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            System.Drawing.Image image = qrCodeEncoder.Encode(url);



        }

      
    }
}