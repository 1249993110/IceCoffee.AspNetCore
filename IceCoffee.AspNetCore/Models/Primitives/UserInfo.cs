using IceCoffee.AspNetCore.Jwt;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace IceCoffee.AspNetCore.Models.Primitives
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
        public string[]? RoleNames { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string? PhoneNumber;

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
                    case JwtRegisteredClaimNames.UserId:
                        UserId = Guid.Parse(claim.Value);
                        break;
                    case JwtRegisteredClaimNames.UserName:
                        UserName = claim.Value;
                        break;
                    case JwtRegisteredClaimNames.DisplayName:
                        DisplayName = claim.Value;
                        break;
                    case JwtRegisteredClaimNames.RoleNames:
                        RoleNames = claim.Value.Split(',');
                        break;
                    case JwtRegisteredClaimNames.Email:
                        Email = claim.Value;
                        break;
                    case JwtRegisteredClaimNames.PhoneNumber:
                        PhoneNumber = claim.Value;
                        break;
                }
            }
        }

        private List<Claim> InternalToClaims()
        {
            var claims = new List<Claim>();

            if (this.UserId.HasValue)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.UserId, this.UserId.Value.ToString()));
            }

            if (this.UserName != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.UserName, this.UserName));
            }

            if (this.DisplayName != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.DisplayName, this.DisplayName));
            }

            if (this.RoleNames != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.RoleNames, string.Join(',', this.RoleNames)));
            }

            if (this.Email != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, this.Email));
            }

            if (this.PhoneNumber != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.PhoneNumber, this.PhoneNumber));
            }

            return claims;
        }

        /// <summary>
        /// 转换为声明
        /// </summary>
        /// <returns></returns>
        public virtual List<Claim> ToClaims()
        {
            return InternalToClaims();
        }

        /// <summary>
        /// 转换为声明
        /// </summary>
        /// <param name="areas">附加的区域</param>
        /// <param name="httpMethods">附加的Http方法</param>
        /// <returns></returns>
        public virtual List<Claim> ToClaims(IEnumerable<string> areas, IEnumerable<string> httpMethods)
        {
            var claims = InternalToClaims();

            if(areas != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Areas, string.Join(',', areas)));
            }

            if (httpMethods != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.HttpMethods, string.Join(';', httpMethods)));
            }

            return claims;
        }
    }
}
