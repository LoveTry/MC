using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
namespace MC
{
    /// <summary>
    /// 消息模板接口
    /// </summary>
    public interface IMsgTemplate
    {

    }

    /// <summary>
    /// 数据结构-待办任务提醒
    /// </summary>
    public class 业务进度提醒 : IMsgTemplate
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    /// <summary>
    /// 数据结构-流程待审批提醒
    /// </summary>
    public class 流程待审批提醒 : IMsgTemplate
    {
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 申请人部门
        /// </summary>
        public TemplateDataItem keyword4 { get; set; }
        /// <summary>
        /// 待审批事件
        /// </summary>
        public TemplateDataItem keyword5 { get; set; }
        public TemplateDataItem remark { get; set; }
    }


    /// <summary>
    /// 数据结构 
    /// </summary>
    public class TemplateDataForMsgApproveResultWarn : IMsgTemplate
    {
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 审批事项
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 审批结果
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem remark { get; set; }
    }


    public class 绑定申请提醒 : IMsgTemplate
    {
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    public class 绑定审核通知 : IMsgTemplate
    {
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    public class 货物消息提醒 : IMsgTemplate
    {
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 提醒时间
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 提醒内容
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    public class 服务评价通知 : IMsgTemplate
    {
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 工作单号
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 提醒时间
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    public class 审批结果提醒 : IMsgTemplate
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 审批事项
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 审批结果
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public TemplateDataItem remark { get; set; }

    }

    public class 提醒通用模板 : IMsgTemplate
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 通知内容
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 通知时间
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public TemplateDataItem remark { get; set; }
    }

    public class 意见建议反馈通知 : IMsgTemplate
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 意见标题
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 提出时间
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 反馈内容
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 回复人
        /// </summary>
        public TemplateDataItem keyword4 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public TemplateDataItem remark { get; set; }
    }

    public class 集装箱动态通知 : IMsgTemplate
    {
        /// <summary>
        /// 信息
        /// </summary>
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 动态
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public TemplateDataItem remark { get; set; }
    }
}
