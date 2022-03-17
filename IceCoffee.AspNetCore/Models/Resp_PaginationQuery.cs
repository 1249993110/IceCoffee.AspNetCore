using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.AspNetCore.Models
{
    public class Resp_PaginationQuery
    {
        /// <summary>
        /// 结果总条数
        /// </summary>
        public uint Total { get; set; }

        /// <summary>
        /// 结果项
        /// </summary>
        public object? Items { get; set; }
    }

    public class Resp_PaginationQuery<T>
    {
        /// <summary>
        /// 结果总条数
        /// </summary>
        public uint Total { get; set; }

        /// <summary>
        /// 结果项
        /// </summary>
        public IEnumerable<T>? Items { get; set; }
    }
}
