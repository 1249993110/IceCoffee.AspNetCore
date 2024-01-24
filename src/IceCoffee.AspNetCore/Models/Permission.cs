namespace IceCoffee.AspNetCore.Models
{
    /// <summary>
    /// Permission
    /// </summary>
    public struct Permission
    {
        /// <summary>
        /// 斜杆: '/' 开头, '/' 代表允许所有子路径
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 逗号: ',' 分割的Http方法, 星号: '*' 代表允许所有方法
        /// </summary>
        public string AllowedHttpMethods { get; set; }
    }
}
