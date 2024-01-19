using System.ComponentModel.DataAnnotations;

namespace IceCoffee.AspNetCore.Options
{
    /// <summary>
    /// AliCloudSmsOptions
    /// </summary>
    public class AliCloudSmsOptions
    {
        /// <summary>
        /// AccessKeyId
        /// </summary>
        [Required]
        public string AccessKeyId { get; set; } = null!;

        /// <summary>
        /// AccessKeySecret
        /// </summary>
        [Required]
        public string AccessKeySecret { get; set; } = null!;

        /// <summary>
        /// SignName
        /// </summary>
        [Required]
        public string SignName { get; set; } = null!;

        /// <summary>
        /// TemplateCode
        /// </summary>
        [Required]
        public string TemplateCode { get; set; } = null!;
    }
}
