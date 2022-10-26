﻿using System.ComponentModel;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class PaginationQueryModel<TOrder> where TOrder: struct, Enum
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DefaultValue(1)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页数量, 值小于 0 时返回所有记录
        /// </summary>
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TOrder? Order { get; set; }

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool Desc { get; set; }
    }
}