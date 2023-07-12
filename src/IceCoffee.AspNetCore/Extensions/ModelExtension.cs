using IceCoffee.AspNetCore.Models;
using IceCoffee.DbCore.Dtos;

namespace IceCoffee.AspNetCore.Extensions
{
    public static class ModelExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="keywordMappedColumnNames">关键词对应的字段名称数组</param>
        /// <param name="preWhereBy">前置 where 条件字符串</param>
        /// <returns></returns>
        public static PaginationQueryDto ToDto(PaginationQueryModel model, string[]? keywordMappedColumnNames = null, string? preWhereBy = null)
        {
            return new PaginationQueryDto()
            {
                Desc = model.Desc,
                Keyword = model.Keyword,
                Order = model.Order,
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                KeywordMappedColumnNames = keywordMappedColumnNames,
                PreWhereBy = preWhereBy
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="keywordMappedColumnNames">关键词对应的字段名称数组</param>
        /// <param name="preWhereBy">前置 where 条件字符串</param>
        /// <returns></returns>
        public static PaginationQueryDto ToDto<TOrder>(PaginationQueryModel<TOrder> model, 
            string[]? keywordMappedColumnNames = null, string? preWhereBy = null) where TOrder : Enum
        {
            return new PaginationQueryDto()
            {
                Desc = model.Desc,
                Keyword = model.Keyword,
                Order = model.Order.ToString(),
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                KeywordMappedColumnNames = keywordMappedColumnNames,
                PreWhereBy = preWhereBy
            };
        }
    }
}