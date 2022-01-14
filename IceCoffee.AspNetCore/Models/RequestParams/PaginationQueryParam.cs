using System.ComponentModel.DataAnnotations;

namespace IceCoffee.AspNetCore.Models.RequestParams
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class PaginationQueryParam
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string? Order { get; set; }

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool Desc { get; set; }
    }
}