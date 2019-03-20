using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using MCComm;
using System.ComponentModel.DataAnnotations;

namespace MC.Models
{

    [ActiveRecord("t_BasicInfoContent")]
    public class BasicInfoContent : ActiveRecordBase<BasicInfoContent>
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
        [Display(Name = "类型")]
        public int TypeID
        {
            get;
            set;
        }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Property]
        [Display(Name = "名称")]
        public string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 排列序号
        /// </summary>
        [Property]
        [Display(Name = "排列")]
        public int SequenceOrder
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
        public string BasicTypeName { get; set; }

        public static DataTable GetBasicContentList(string where = "1=1")
        {
            string sql = "SELECT * FROM dbo.v_BasicContentList where {0}".FormatWith(where);
            return Sunnysoft.DAL.ActiveRecordDBHelper.ExecuteDatatable(sql);
        }

        public static int GetMaxSeq()
        {
            string sql = "SELECT ISNULL(MAX(SequenceOrder),0) FROM dbo.t_BasicInfoContent";
            return Sunnysoft.DAL.ActiveRecordDBHelper.ExecuteScalar(sql).ToString().ToInt();
        }
    }
}