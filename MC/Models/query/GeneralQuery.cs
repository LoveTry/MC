using MCComm;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace MC.Models
{
    /// <summary>
    /// 通用检索添加查询类
    /// </summary>
    public class GeneralQuery
    {
        public string no { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public int state { get; set; }

        public GeneralQuery() { }

        /// <summary>
        /// 通过Request.QuertString构建GeneralQuery
        /// </summary>
        /// <param name="nameValue"></param>
        /// <returns></returns>
        public static GeneralQuery Get(NameValueCollection nameValue)
        {
            var model = new GeneralQuery();
            foreach (var item in typeof(GeneralQuery).GetProperties())
            {
                if (nameValue.AllKeys.Contains(item.Name))
                {
                    if (item.PropertyType == typeof(int))
                        item.SetValue(model, nameValue[item.Name].ToInt());
                    else
                        item.SetValue(model, nameValue[item.Name]);
                }
            }
            return model;
        }
    }

    public enum UseState
    {
        ALL = 1,
        Use = 2,
        No = 3
    }
}