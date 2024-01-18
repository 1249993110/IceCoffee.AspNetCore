using Microsoft.Extensions.Configuration;
using Tea;

namespace IceCoffee.AspNetCore.Services
{
    public class AlibabaCloudService
    {
        private readonly IConfiguration _configuration;

        public AlibabaCloudService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient()
        {
            var config = new AlibabaCloud.OpenApiClient.Models.Config()
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = _configuration.GetValue<string>("AlibabaCloudOptions:AccessKeyId"),
                // 必填，您的 AccessKey Secret
                AccessKeySecret = _configuration.GetValue<string>("AlibabaCloudOptions:AccessKeySecret"),
            };
            // Endpoint 请参考 https://api.aliyun.com/product/Dysmsapi
            config.Endpoint = "dysmsapi.aliyuncs.com";
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
        }

        public Task SendAsync(string phoneNumbers, string code)
        {
            var client = CreateClient();
            var sendSmsRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                SignName = _configuration.GetValue<string>("AlibabaCloudOptions:SignName"),
                TemplateCode = _configuration.GetValue<string>("AlibabaCloudOptions:TemplateCode"),
                PhoneNumbers = phoneNumbers,
                TemplateParam = "{\"code\":\"" + code + "\"}",
            };

            var runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
            return client.SendSmsWithOptionsAsync(sendSmsRequest, runtime);
        }
    }
}
