using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Models
{
    public class Const
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        public const string SESSION_USRE_INFO = "SESSION_USRE_INFO";

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public const string PC_USRE_INFO = "PC_USRE_INFO";
    }

    public enum AjaxReturnCode
    {
        OK = 1,
        ERROR = 2,
        WARNNING = 3
    }
}