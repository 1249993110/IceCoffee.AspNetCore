using System.Security.Claims;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public IEnumerable<string>? RoleNames { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 许可
        /// </summary>
        public IEnumerable<Permission>? Permissions { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<Tag>? Tags { get; set; }

        /// <summary>
        /// 默认构造
        /// </summary>
        public UserInfo()
        {
        }

        /// <summary>
        /// 从声明初始化实例
        /// </summary>
        /// <param name="claims"></param>
        public UserInfo(IEnumerable<Claim> claims)
        {
            var permissions = new List<Permission>();
            var tags = new List<Tag>();
            foreach (var claim in claims)
            {
                if(claim.Type == RegisteredClaimNames.UserId)
                {
                    UserId = claim.Value;
                }
                else if (claim.Type == RegisteredClaimNames.UserName)
                {
                    UserName = claim.Value;
                }
                else if (claim.Type == RegisteredClaimNames.DisplayName)
                {
                    DisplayName = claim.Value;
                }
                else if (claim.Type == RegisteredClaimNames.RoleNames)
                {
                    RoleNames = claim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
                }
                else if (claim.Type == RegisteredClaimNames.Email)
                {
                    Email = claim.Value;
                }
                else if (claim.Type == RegisteredClaimNames.PhoneNumber)
                {
                    PhoneNumber = claim.Value;
                }
                else if (claim.Type.StartsWith(RegisteredClaimNames.PermissionPrefix))
                {
                    string uri = claim.Type.Substring(RegisteredClaimNames.PermissionPrefix.Length);
                    permissions.Add(new Permission() { Uri = uri, AllowedHttpMethods = claim.Value });
                }
                else if (claim.Type.StartsWith(RegisteredClaimNames.TagPrefix))
                {
                    string name = claim.Type.Substring(RegisteredClaimNames.TagPrefix.Length);
                    tags.Add(new Tag() { Name = name, Value = claim.Value });
                }
            }

            Permissions = permissions;
            Tags = tags;
        }

        /// <summary>
        /// 转换为声明
        /// </summary>
        /// <returns></returns>
        public virtual List<Claim> ToClaims()
        {
            var claims = new List<Claim>();

            if (this.UserId != null)
            {
                claims.Add(new Claim(RegisteredClaimNames.UserId, this.UserId));
            }

            if (this.UserName != null)
            {
                claims.Add(new Claim(RegisteredClaimNames.UserName, this.UserName));
            }

            if (this.DisplayName != null)
            {
                claims.Add(new Claim(RegisteredClaimNames.DisplayName, this.DisplayName));
            }

            if (this.RoleNames != null)
            {
                claims.Add(new Claim(RegisteredClaimNames.RoleNames, string.Join(',', this.RoleNames)));
            }

            if (this.Email != null)
            {
                claims.Add(new Claim(RegisteredClaimNames.Email, this.Email));
            }

            if (this.PhoneNumber != null)
            {
                claims.Add(new Claim(RegisteredClaimNames.PhoneNumber, this.PhoneNumber));
            }

            if (this.Permissions != null)
            {
                foreach (var item in this.Permissions)
                {
                    claims.Add(new Claim(RegisteredClaimNames.PermissionPrefix + item.Uri, item.AllowedHttpMethods));
                }
            }

            if (this.Tags != null)
            {
                foreach (var item in this.Tags)
                {
                    claims.Add(new Claim(RegisteredClaimNames.TagPrefix + item.Name, item.Value));
                }
            }

            return claims;
        }
    }
}