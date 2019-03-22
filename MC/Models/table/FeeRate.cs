using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace MC.Models
{

    [ActiveRecord("t_FeeRate")]
    public class FeeRate : ActiveRecordBase<FeeRate>
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
        /// 比例
        /// </summary>
        [Property]
        [Display(Name = "佣金比例(小数)")]
        public decimal Rate
        {
            get;
            set;
        }

        /// <summary>
        /// 额外扣除
        /// </summary>
        [Property]
        [Display(Name = "额外扣除")]
        public decimal Other
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

    }
}