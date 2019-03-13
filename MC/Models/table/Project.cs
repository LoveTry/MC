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
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// 产品描述
		/// </summary>
		[Property]
		public string Description
		{
			get;
			set;
		}

		/// <summary>
		/// 单价
		/// </summary>
		[Property]
		public decimal Price
		{
			get;
			set;
		}

		/// <summary>
		/// 数量
		/// </summary>
		[Property]
		public int Amount
		{
			get;
			set;
		}

		/// <summary>
		/// 总价
		/// </summary>
		[Property]
		public decimal Total
		{
			get;
			set;
		}

		/// <summary>
		/// 单位
		/// </summary>
		[Property]
		public string Unit
		{
			get;
			set;
		}

		/// <summary>
		/// 有效期始
		/// </summary>
		[Property]
		public DateTime? StartDate
		{
			get;
			set;
		}

		/// <summary>
		/// 有效期至
		/// </summary>
		[Property]
		public DateTime? EndDate
		{
			get;
			set;
		}

		/// <summary>
		/// 折扣
		/// </summary>
		[Property]
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
		/// 更新人
		/// </summary>
		[Property]
		public string UpUser
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

		#endregion

		
	}
}