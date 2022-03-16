using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.AspNetCore.Options
{
    public class EmailSendOptions
    {
        /// <summary>
        /// 是否是html格式
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// 邮件标题
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// 发送方邮箱地址
        /// </summary>
        public string? FromAddress { get; set; }

        /// <summary>
        /// 发送方用户昵称
        /// </summary>
        public string? FromDisplayName { get; set; }

        /// <summary>
        /// 模板文件路径
        /// </summary>
        public string? TemplateFilePath { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string? Captcha { get; set; }

        /// <summary>
        /// 目标邮箱地址
        /// </summary>
        public string? ToAddress { get; set; }

        /// <summary>
        /// 目标用户昵称
        /// </summary>
        public string? ToDisplayName { get; set; }

        /// <summary>
        /// 当前日期时间
        /// </summary>
        public string? CurrentDateTime { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string? AccountName { get; set; }
    }
}
