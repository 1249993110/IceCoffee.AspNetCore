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
        public Guid? UserId { get; set; }

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
        /// 授权的区域
        /// </summary>
        public IEnumerable<string>? Areas { get; set; }

        public UserInfo()
        {
        }

        /// <summary>
        /// 从声明初始化实例
        /// </summary>
        /// <param name="claims"></param>
        public UserInfo(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                switch (claim.Type)
                {
                    case RegisteredClaimNames.UserId:
                        UserId = Guid.Parse(claim.Value);
                        break;

                    case RegisteredClaimNames.UserName:
                        UserName = claim.Value;
                        break;

                    case RegisteredClaimNames.DisplayName:
                        DisplayName = claim.Value;
                        break;

                    case RegisteredClaimNames.RoleNames:
                        RoleNames = claim.Value.Split(',');
                        break;

                    case RegisteredClaimNames.Email:
                        Email = claim.Value;
                        break;

                    case RegisteredClaimNames.PhoneNumber:
                        PhoneNumber = claim.Value;
                        break;

                    case RegisteredClaimNames.Areas:
                        Areas = claim.Value.Split(',');
                        break;
                }
            }
        }

        /// <summary>
        /// 转换为声明
        /// </summary>
        /// <returns></returns>
        public virtual List<Claim> ToClaims()
        {
            var claims = new List<Claim>();

            if (this.UserId.HasValue)
            {
                claims.Add(new Claim(RegisteredClaimNames.UserId, this.UserId.Value.ToString()));
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

            if (this.Areas != null)
            {
                claims.Add(new Claim(RegisteredClaimNames.Areas, string.Join(',', this.Areas)));
            }

            return claims;
        }
    }
}