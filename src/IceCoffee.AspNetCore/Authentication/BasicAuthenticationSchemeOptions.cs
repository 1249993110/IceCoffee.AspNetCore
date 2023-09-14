using Microsoft.AspNetCore.Authentication;

namespace IceCoffee.AspNetCore.Authentication
{
    /// <summary>
    /// Basic 认证选项
    /// </summary>
    public class BasicAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}