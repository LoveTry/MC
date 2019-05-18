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

	[ActiveRecord("t_ProjectIntroduce")]
	public class ProjectIntroduce : ActiveRecordBase<ProjectIntroduce>
	{

		#region

		/// <summary>
		/// 主键ID（项目表主键）
		/// </summary>
		[PrimaryKey(PrimaryKeyType.Assigned)]
		public Guid ID
		{
			get;
			set;
		}

		/// <summary>
		/// 内容
		/// </summary>
		[Property]
        [Display(Name = "简介内容")]
        public string Content
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
		/// 更新人
		/// </summary>
		[Property]
        [Display(Name = "更新人")]
        public string UpUser
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

		#endregion

	}
}