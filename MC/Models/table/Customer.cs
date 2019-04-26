using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using NHibernate.Criterion;
using System.ComponentModel.DataAnnotations;
using MCComm;

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
        [Required]
        public string CusName
        {
            get;
            set;
        }

        /// <summary>
        /// 客户电话
        /// </summary>
        [Property]
        [Required]
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
        /// 记录选择项目(用逗号隔开)
        /// </summary>
        public string ChooseIDList
        {
            get; set;
        }

        public static Customer FindByProperty(string property, object value)
        {
            ICriterion exp = Restrictions.Eq(property, value);
            return Customer.FindOne(exp);
        }

        public static Customer FindByIdAndCrId(Guid CusID, Guid CrUserID)
        {
            ICriterion exp = Restrictions.Eq("ID", CusID);
            exp = Restrictions.And(exp, Restrictions.Eq("CrUserID", CrUserID));
            return FindOne(exp);
        }

        public static DataTable GetCustomerList(string where = "1=1")
        {
            string sql = @"SELECT *,
       (
           SELECT COUNT(ID) FROM dbo.t_Order WHERE t_Order.CusID = t_Customer.ID
       ) AS OrderNum
FROM dbo.t_Customer where {0} ORDER BY CrTime DESC".FormatWith(where);
            return Sunnysoft.DAL.ActiveRecordDBHelper.ExecuteDatatable(sql);
        }

        public static Customer[] GetCustomerList()
        {
            DetachedCriteria exp = DetachedCriteria.For(typeof(Customer));
            exp.AddOrder(NHibernate.Criterion.Order.Desc("CrTime"));
            return FindAll(exp);
        }
    }
}