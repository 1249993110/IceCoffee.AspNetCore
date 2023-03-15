using IceCoffee.DbCore.Dtos;

namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationQueryResult<T>
    {
        /// <summary>
        /// 结果总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 结果项
        /// </summary>
        public IEnumerable<T>? Items { get; set; }

        /// <summary>
        /// Implicitly converts the specified <paramref name="dto"/> to an <see cref="PaginationResultDto{T}"/>.
        /// </summary>
        /// <param name="dto">The value to convert.</param>
        public static implicit operator PaginationQueryResult<T>(PaginationResultDto<T> dto)
        {
            return new PaginationQueryResult<T>()
            {
                Total = dto.Total,
                Items = dto.Items
            };
        }
    }
}