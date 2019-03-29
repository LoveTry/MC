using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using MCComm;

namespace MC.Models
{

    [ActiveRecord("t_Fee")]
    public class Fee : ActiveRecordBase<Fee>
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
        /// 订单ID
        /// </summary>
        [Property]
        public int OrderID
        {
            get;
            set;
        }

        /// <summary>
        /// 佣金金额
        /// </summary>
        [Property]
        public decimal Money
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已支付
        /// </summary>
        [Property]
        public bool IsPay
        {
            get;
            set;
        }

        /// <summary>
        /// 推荐人ID
        /// </summary>
        [Property]
        public Guid PayeeID
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人ID
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
        /// 审核人ID
        /// </summary>
        [Property]
        public Guid ApproverID
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
            get; set;
        }

        #endregion

        public string PayeeUserName { get; set; }
        public string PayeeTrueName { get; set; }
        public string OrderNo { get; set; }
        public static DataTable GetFeeListAll(string where = "1=1")
        {
            string sql = "SELECT * FROM v_FeeList where {0} ORDER BY CrUserID,CrTime DESC".FormatWith(where);
            return Sunnysoft.DAL.ActiveRecordDBHelper.ExecuteDatatable(sql);
        }

        /// <summary>
        /// 获取用户结佣金额
        /// </summary>
        /// <param name="IsPay">是否已付</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public static decimal GetFee(bool IsPay, Guid userid)
        {
            var feeQuery = FindAllByProperty("PayeeID", userid);
            if (feeQuery != null && feeQuery.Count() > 0)
            {
                return feeQuery.Where(w => w.IsPay == IsPay).Sum(f => f.Money);
            }
            return 0;
        }

    }
}