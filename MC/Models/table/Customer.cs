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

    [ActiveRecord("t_Customer")]
    public class Customer : ActiveRecordBase<Customer>
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
        /// 客户名称
        /// </summary>
        [Property]
        public string CusName
        {
            get;
            set;
        }

        /// <summary>
        /// 客户电话
        /// </summary>
        [Property]
        public string CusPhone
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
        /// 描述
        /// </summary>
        [Property]
        public string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 创建ID
        /// </summary>
        [Property]
        public Guid CrUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人
        /// </summary>
        [Property]
        public string CrUser
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
        /// 更新时间
        /// </summary>
        [Property]
        public DateTime? UpTime
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// 记录选择项目
        /// </summary>
        public List<string> ChooseIDList
        {
            get;set;
        }

        public Customer FindByProperty(string property, object value)
        {
            ICriterion exp = Restrictions.Eq(property, value);
            return Customer.FindOne(exp);
        }
    }
}