using IceCoffee.AspNetCore.Models.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IceCoffee.AspNetCore
{
    /// <summary>
    /// 成功响应数据类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SucceededResponseTypeAttribute : ProducesResponseTypeAttribute
    {
        public SucceededResponseTypeAttribute(Type type) : base(GetType(type), StatusCodes.Status200OK)
        {
        }

        private static Type GetType(Type type)
        {
            if (typeof(IResponse).IsAssignableFrom(type))
            {
                return type;
            }

            return typeof(Response<>).MakeGenericType(type);
        }
    }
}
