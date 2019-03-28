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

	[ActiveRecord("t_Setting")]
	public class Setting : ActiveRecordBase<Setting>
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
		/// 活动规则
		/// </summary>
		[Property]
		public string ActivityRule
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
	}
}