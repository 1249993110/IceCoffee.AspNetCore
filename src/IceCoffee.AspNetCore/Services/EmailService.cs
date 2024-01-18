using IceCoffee.AspNetCore.Models;
using IceCoffee.AspNetCore.Options;
using IceCoffee.Common.Templates;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace IceCoffee.AspNetCore.Services
{
    public class EmailService
    {
        private readonly SmtpOptions _smtpOptions;

        public EmailService(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public virtual async Task SendAsync(EmailSendArgs emailSendArgs)
        {
            try
            {
                if (emailSendArgs == null)
                {
                    throw new ArgumentNullException(nameof(emailSendArgs));
                }

                if (string.IsNullOrEmpty(emailSendArgs.FromAddress))
                {
                    throw new ArgumentNullException(nameof(EmailSendArgs.FromAddress));
                }

                if (string.IsNullOrEmpty(emailSendArgs.ToAddress))
                {
                    throw new ArgumentNullException(nameof(EmailSendArgs.ToAddress));
                }

                if (string.IsNullOrEmpty(emailSendArgs.TemplateFilePath))
                {
                    throw new ArgumentNullException(nameof(EmailSendArgs.TemplateFilePath));
                }

                // 初始化发送邮件对象
                var client = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
                {
                    // 是否启用SSL
                    EnableSsl = _smtpOptions.EnableSsl,
                    // 是否启用身份验证（UseDefaultCredentials属性必须要在Credentials前配置, 否则会报错）
                    UseDefaultCredentials = _smtpOptions.UseDefaultCredentials,
                    // 配置身份账号密码
                    Credentials = new NetworkCredential(_smtpOptions.UserName, _smtpOptions.Password)
                };

                string body = StringTemplate.Render(System.IO.File.ReadAllText(emailSendArgs.TemplateFilePath), emailSendArgs);

                // 要发送的邮件对象
                var email = new MailMessage()
                {
                    // 发件人邮箱和展示名称
                    From = new MailAddress(emailSendArgs.FromAddress, emailSendArgs.FromDisplayName, Encoding.UTF8),
                    // 是否是html格式
                    IsBodyHtml = emailSendArgs.IsBodyHtml,
                    // 邮件标题
                    Subject = emailSendArgs.Subject,
                    // 邮件内容编码
                    BodyEncoding = Encoding.UTF8,
                    // 邮件内容
                    Body = body,
                    // 邮件优先级
                    Priority = MailPriority.Normal
                };
                // 收件人（可以多个）
                email.To.Add(emailSendArgs.ToAddress);

                AsyncCompletedEventArgs? result = null;

                // 发送完毕事件（只针对异步发送有效）
                client.SendCompleted += (sender, e) =>
                {
                    result = e;
                };

                // 发送邮件
                await client.SendMailAsync(email);

                if (result == null)
                {
                    throw new Exception("发送邮件异常：异步完成事件参数 AsyncCompletedEventArgs 为空");
                }
                else
                {
                    if (result.Error != null)
                    {
                        throw new Exception("发送邮件异常：", result.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("发送邮件异常：", ex);
            }
        }
    }
}