using IceCoffee.AspNetCore.Jwt;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace IceCoffee.AspNetCore.Models.Primitives
{
    public class UserInfoBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string? UserId;

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName;

        /// <summary>
        /// 显示名称
        /// </summary>
        public string? DisplayName;

        /// <summary>
        /// 角色名
        /// </summary>
        public string[]? RoleNames;

        /// <summary>
        /// Email
        /// </summary>
        public string? Email;

        /// <summary>
        /// 电话号码
        /// </summary>
        public string? PhoneNumber;

        public UserInfoBase()
        {
        }

        /// <summary>
        /// 从声明初始化实例
        /// </summary>
        /// <param name="claims"></param>
        public UserInfoBase(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                switch (claim.Type)
                {
                    case JwtRegisteredClaimNames.UserId:
                        UserId = claim.Value;
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

        public virtual List<Claim> ToClaims()
        {
            var claims = new List<Claim>();

            if(this.UserId != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.UserId, this.UserId));
            }

            if(this.UserName != null)
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
    }
}
