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

    [ActiveRecord("t_Order")]
    public class Order : ActiveRecordBase<Order>
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
        /// 流水号
        /// </summary>
        [Property]
        public string OrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// 客户ID
        /// </summary>
        [Property]
        public Guid CusID
        {
            get;
            set;
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Property]
        public Guid ProID
        {
            get;
            set;
        }

        /// <summary>
        /// 产品价格
        /// </summary>
        [Property]
        public decimal ProMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        [Property]
        public string State
        {
            get;
            set;
        }

        /// <summary>
        /// 状态信息
        /// </summary>
        [Property]
        public string StateInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 推荐人ID
        /// </summary>
        [Property]
        public Guid CrUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 推荐时间
        /// </summary>
        [Property]
        public DateTime? CrTime
        {
            get;
            set;
        }

        #endregion
    }
}