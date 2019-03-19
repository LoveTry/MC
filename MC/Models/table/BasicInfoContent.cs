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
		public int TypeID
		{
			get;
			set;
		}

		/// <summary>
		/// 类型名称
		/// </summary>
		[Property]
		public string TypeName
		{
			get;
			set;
		}

		/// <summary>
		/// 排列序号
		/// </summary>
		[Property]
		public int SequenceOrder
		{
			get;
			set;
		}

		/// <summary>
		/// 禁用标记
		/// </summary>
		[Property]
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

		/// <summary>
		/// 更新人
		/// </summary>
		[Property]
		public string UpUser
		{
			get;
			set;
		}

        #endregion

        public static DataTable GetBasicContent(string where)
        {
            string sql = "SELECT * FROM dbo.t_BasicInfoType where {0}".FormatWith(where);
            return Sunnysoft.DAL.ActiveRecordDBHelper.ExecuteDatatable(sql);
        }

		
	}
}