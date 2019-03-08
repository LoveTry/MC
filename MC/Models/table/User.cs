using Castle.ActiveRecord;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MC.Models
{
    [ActiveRecord("t_User")]
    public class User : ActiveRecordBase<User>
    {
        #region
        /// <summary>
        /// 用户ID
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned)]
        public Guid UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Property]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Property]
        public string TrueName
        {
            get;
            set;
        }

        /// <summary>
        /// 微信openid
        /// </summary>
        [Property]
        public string OpenID
        {
            get;
            set;
        }

        /// <summary>
        /// 性别
        /// </summary>
        [Property]
        public string Sex
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        [Property]
        public string Phone
        {
            get;
            set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Property]
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property]
        public DateTime? CrTime
        {
            get;
            set;
        }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Property]
        public DateTime? LastOnLineTime
        {
            get;
            set;
        }
        #endregion

        public static User FindByProperty(string property, object value)
        {
            ICriterion exp = Expression.Eq(property, value);
            return User.FindOne(exp);
        }
    }
}
