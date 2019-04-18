using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Models
{
    public class JsonReturn
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public object data { get; set; }

        public JsonReturn() { }

        /// <summary>
        /// 构建Json返回数据
        /// </summary>
        /// <param name="data">数据集</param>
        /// <param name="page">当前页（从1开始）</param>
        /// <param name="limit">每页数</param>
        public JsonReturn(IEnumerable<object> data, int page, int limit)
        {
            code = 0;
            msg = "";
            this.count = data.Count();
            this.data = data.Skip((page - 1) * limit).Take(limit).ToList();
        }

        public static JsonReturn OK()
        {
            return new JsonReturn() { code = 0, msg = "" };
        }

        public static JsonReturn Error(string msg)
        {
            return new JsonReturn() { code = 1, msg = msg };
        }
    }


}