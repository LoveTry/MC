using MC.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC.Controllers
{
    /// <summary>
    /// 客户资料管理
    /// </summary>
    [RoutePrefix("api/card")]
    public class CardShareController : Controller
    {
        // GET: CardShare
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public string CreateCard(JObject obj)
        {
            try
            {
                long maxticks = CheckData.Check_Long(obj["maxticks"]);
                int maxcount = CheckData.Check_Int(obj["maxcount"]);
                int offset = CheckData.Check_Int(obj["offset"]);
                int isZip = CheckData.Check_Int(obj["zip"]);
                var db = new CRMData(CurrentAccount.accountid);
                var crm = db.GetCrmItemList(maxticks, maxcount, offset);
                if (isZip == 1)
                {
                    string str = GZipUtil.CompressString(JsonConvert.SerializeObject(crm));
                    return Ok(ResponseResult.Success((object)str));
                }
                else
                {
                    return Ok(ResponseResult.Success(crm));
                }
            }
            catch (Exception e)
            {
                return Ok(ResponseResult.Error());
            }
        }
    }
}