using IceCoffee.AspNetCore.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Tea;

namespace IceCoffee.AspNetCore.Services
{
    /// <summary>
    /// 阿里云短信服务
    /// </summary>
    public class AliCloudSmsService
    {
        private readonly AliCloudSmsOptions _options;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="options"></param>
        public AliCloudSmsService(IOptions<AliCloudSmsOptions> options)
        {
            _options = options.Value;
        }

        private AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient()
        {
            var config = new AlibabaCloud.OpenApiClient.Models.Config()
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = _options.AccessKeyId,
                // 必填，您的 AccessKey Secret
                AccessKeySecret = _options.AccessKeySecret,
            };
            // Endpoint 请参考 https://api.aliyun.com/product/Dysmsapi
            config.Endpoint = "dysmsapi.aliyuncs.com";
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task SendAsync(string phoneNumbers, string code)
        {
            var client = CreateClient();
            var sendSmsRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                SignName = _options.SignName,
                TemplateCode = _options.TemplateCode,
                PhoneNumbers = phoneNumbers,
                TemplateParam = string.Concat("{\"code\":\"", code, "\"}"),
            };

            var runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
            return client.SendSmsWithOptionsAsync(sendSmsRequest, runtime);
        }
    }
}
