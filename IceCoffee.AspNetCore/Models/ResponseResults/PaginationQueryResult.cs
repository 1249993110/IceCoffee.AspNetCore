using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.AspNetCore.Models.ResponseResults
{
    public class PaginationQueryResult
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

    public class PaginationQueryResult<T>
    {
        /// <summary>
        /// 结果总条数
        /// </summary>
        public uint Total { get; set; }

        /// <summary>
        /// 结果项
        /// </summary>
        public T? Items { get; set; }
    }
}
