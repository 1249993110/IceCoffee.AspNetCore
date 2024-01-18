using System.ComponentModel;
using System.Text.Json.Serialization;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class PaginationQuery
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DefaultValue(1)]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// 页大小
        /// </summary>
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string? Keyword { get; set; }
    }

    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class PaginationQuery<TOrder> : PaginationQuery where TOrder: Enum
    {
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