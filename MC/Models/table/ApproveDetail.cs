using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace MC.Models
{

    [ActiveRecord("t_ApproveDetail")]
    public class ApproveDetail : ActiveRecordBase<ApproveDetail>
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
        /// 审核类型
        /// </summary>
        [Property]
        public int ApproveType
        {
            get;
            set;
        }

        /// <summary>
        /// 业务主键(int)
        /// </summary>
        [Property]
        public int BusinessIntID
        {
            get;
            set;
        }

        /// <summary>
        /// 业务主键(guid)
        /// </summary>
        [Property]
        public Guid BusinessGuidID
        {
            get;
            set;
        }

        /// <summary>
        /// 审核人
        /// </summary>
        [Property]
        public string Approver
        {
            get;
            set;
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Property]
        public DateTime? ApproveTime
        {
            get;
            set;
        }

        /// <summary>
        /// 审核结果
        /// </summary>
        [Property]
        public bool IsOK
        {
            get;
            set;
        }

        /// <summary>
        /// 审核信息
        /// </summary>
        [Property]
        public string ApproveMsg
        {
            get;
            set;
        }

        #endregion
    }

    public enum ApproveType
    {
        ORDER = 1,
        FEE = 2
    }
}