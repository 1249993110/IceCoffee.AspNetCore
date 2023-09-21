namespace IceCoffee.AspNetCore.Models
{
    public class Paged<T>
    {
        /// <summary>
        /// 总计
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}
