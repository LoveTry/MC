using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Sunnysoft.DAL;

namespace MC.Models
{
    public class ReportTotal
    {
        /// <summary>
        /// 课程订单总计
        /// </summary>
        public static DataTable GetProNameAndFee()
        {
            string sql = @"SELECT t_Project.Name,
                           Convert(decimal(18,2),SUM(t_Order.ProMoney)) AS Fee
                    FROM dbo.t_Order
                        INNER JOIN t_Project
                            ON t_Project.ID = t_Order.ProID
                    GROUP BY t_Project.Name";
            return ActiveRecordDBHelper.ExecuteDatatable(sql);
        }
    }
}