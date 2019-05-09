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
    /// 数据结构-订单生成通知
    /// </summary>
    public class 订单生成通知 : IMsgTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TemplateDataItem remark { get; set; }
    }

    /// <summary>
    /// 数据结构-推荐成交通知
    /// </summary>
    public class 推荐成交通知 : IMsgTemplate
    {
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 推荐客户
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 成交时间
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 成交佣金
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 成交项目
        /// </summary>
        public TemplateDataItem keyword4 { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    public class 审核结果通知 : IMsgTemplate
    {

        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 审批结果
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem remark { get; set; }

    }
}
