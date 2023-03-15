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
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}