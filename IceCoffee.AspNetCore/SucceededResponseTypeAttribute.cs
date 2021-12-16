using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IceCoffee.AspNetCore
{
    /// <summary>
    /// 成功响应类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SucceededResponseTypeAttribute : ProducesResponseTypeAttribute
    {
        public SucceededResponseTypeAttribute(Type type) : base(type, StatusCodes.Status200OK)
        {
        }
    }
}
