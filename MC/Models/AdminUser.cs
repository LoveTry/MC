using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using NHibernate.Criterion;

namespace MC.Models
{

    [ActiveRecord("t_AdminUser")]
    public class AdminUser : ActiveRecordBase<AdminUser>
    {
        #region

        /// <summary>
        /// 主键ID
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned)]
        public Guid ID
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
        /// 密码
        /// </summary>
        [Property]
        public string PassWord
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


        public static AdminUser GetAdminUser(string username, string password)
        {
            ICriterion exp = Restrictions.Eq("UserName", username);
            exp = Restrictions.And(exp, Restrictions.Eq("PassWord", password));
            return FindOne(exp);
        }
    }
}