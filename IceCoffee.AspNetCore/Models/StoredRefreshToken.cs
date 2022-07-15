using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// Stored Refresh Token
    /// </summary>
    public class StoredRefreshToken
    {
        /// <summary>
        /// Refresh Token 的值
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 使用 JwtId 映射到对应的 token
        /// </summary>
        public Guid JwtId { get; set; }

        /// <summary>
        /// 是否出于安全原因已将其撤销
        /// </summary>
        public bool IsRevorked { get; set; }

        /// <summary>
        /// Refresh Token 的生命周期很长, 可以长达数月。注意一个Refresh Token只能被用来刷新一次
        /// </summary>
        public DateTime ExpiryDate { get; set; }
    }
}
