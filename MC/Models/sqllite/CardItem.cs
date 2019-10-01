using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Models.sqllite
{
    public class CardItem
    {
        public CardItem()
        {
            this.createtime = DateTime.Now;
        }
        /// <summary>
        /// 主键
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string wx { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
    }
}