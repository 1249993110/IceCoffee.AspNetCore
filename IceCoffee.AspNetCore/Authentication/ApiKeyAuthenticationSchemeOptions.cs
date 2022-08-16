using Microsoft.AspNetCore.Authentication;

namespace IceCoffee.AspNetCore.Authentication
{
    /// <summary>
    /// ApiKey 认证选项
    /// </summary>
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string? AccessToken { get; set; }
    }
}