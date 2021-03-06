using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using NHibernate.Criterion;
using MCComm;
using NHibernate;

namespace MC.Models
{

    [ActiveRecord("t_Project")]
    public class Project : ActiveRecordBase<Project>
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
        /// 产品名称
        /// </summary>
        [Property]
        [Display(Name = "课程名称")]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 产品描述
        /// </summary>
        [Property]
        [Display(Name = "课程描述")]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 单价
        /// </summary>
        [Property]
        [Display(Name = "单价")]
        public decimal Price
        {
            get;
            set;
        }

        /// <summary>
        /// 数量
        /// </summary>
        [Property]
        [Display(Name = "数量")]
        public int Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 总价
        /// </summary>
        [Property]
        [Display(Name = "总价")]
        public decimal Total
        {
            get;
            set;
        }

        /// <summary>
        /// 单位
        /// </summary>
        [Property]
        [Display(Name = "单位")]
        public string Unit
        {
            get;
            set;
        }

        /// <summary>
        /// 有效期始
        /// </summary>
        [Property]
        [Display(Name = "开课时间")]
        public DateTime? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 有效期至
        /// </summary>
        [Property]
        [Display(Name = "结课时间")]
        public DateTime? EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 折扣
        /// </summary>
        [Property]
        [Display(Name = "折扣")]
        public decimal DisCount
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
        [Display(Name = "创建人")]
        public string CrUser
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property]
        [Display(Name = "创建时间")]
        public DateTime? CrTime
        {
            get;
            set;
        }

        /// <summary>
        /// 更新人
        /// </summary>
        [Property]
        [Display(Name = "更新人")]
        public string UpUser
        {
            get;
            set;
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Property]
        [Display(Name = "更新时间")]
        public DateTime? UpTime
        {
            get;
            set;
        }

        /// <summary>
        /// 更新
        /// </summary>
        [Property]
        [Display(Name = "是否启用")]
        public bool IsUse
        {
            get;
            set;
        }
        #endregion

        public static Project[] GetList(GeneralQuery query = null)
        {
            if (query != null)
            {
                DetachedCriteria exp = DetachedCriteria.For(typeof(Project));
                if (query.date.IsNotEmpty())
                {
                    exp.Add(Restrictions.Ge("StartDate", query.date.ToDateTime()));
                }
                if (query.name.IsNotEmpty())
                {
                    exp.Add(Restrictions.Like("Name", "%" + query.name + "%"));
                }
                switch (query.state)
                {
                    case 1://全部
                        break;
                    case 0://初始化
                    case 2://已启用
                        exp.Add(Restrictions.Eq("IsUse", true));
                        break;
                    case 3://未启用
                        exp.Add(Restrictions.Eq("IsUse", false));
                        break;
                }
                exp.AddOrder(NHibernate.Criterion.Order.Desc("CrTime"));
                return FindAll(exp);
            }
            else
            {
                return FindAll();
            }
        }

        public static Project[] GetChooseList()
        {
            DetachedCriteria exp = DetachedCriteria.For(typeof(Project));
            exp.Add(Restrictions.Eq("IsUse", true));
            exp.AddOrder(NHibernate.Criterion.Order.Desc("CrTime"));
            return FindAll(exp);
        }
    }
}