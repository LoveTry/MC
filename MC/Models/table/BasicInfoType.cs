using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Mvc;
using NHibernate.Criterion;
using MCComm;
using System.ComponentModel.DataAnnotations;

namespace MC.Models
{

    [ActiveRecord("t_BasicInfoType")]
    public class BasicInfoType : ActiveRecordBase<BasicInfoType>
    {

        #region

        /// <summary>
        /// 主键ID
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Identity)]
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// 类型ID
        /// </summary>
        [Property]
        [Display(Name = "类型ID")]
        public int TypeID
        {
            get;
            set;
        }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Property]
        [Display(Name = "类型名称")]
        public string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 禁用标记
        /// </summary>
        [Property]
        [Display(Name = "禁用标记")]
        public bool DelFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Property]
        public Guid CrUserId
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
        /// 更新人
        /// </summary>
        [Property]
        [Display(Name = "更新人")]
        public string UpUser
        {
            get;
            set;
        }

        #endregion

        public static DataTable GetList(string where = "1=1")
        {
            string sql = "SELECT * FROM dbo.t_BasicInfoType where {0}".FormatWith(where);
            return Sunnysoft.DAL.ActiveRecordDBHelper.ExecuteDatatable(sql);
        }

        public static List<SelectListItem> GetTypeList()
        {
            ICriterion exp = Restrictions.Eq("DelFlag", false);
            var list = from type in FindAll(exp).AsEnumerable()
                       select new SelectListItem
                       {
                           Text = type.TypeName,
                           Value = type.TypeID.ToString()
                       };

            return list.ToList();
        }

        public static int GetMaxTypeID()
        {
            string sql = "SELECT ISNULL(MAX(TypeID),0) FROM dbo.t_BasicInfoType";
            return Sunnysoft.DAL.ActiveRecordDBHelper.ExecuteScalar(sql).ToString().ToInt();
        }
    }
}