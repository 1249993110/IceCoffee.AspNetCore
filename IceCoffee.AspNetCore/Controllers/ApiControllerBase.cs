using IceCoffee.AspNetCore.Models;
using IceCoffee.AspNetCore.Models.Primitives;
using IceCoffee.AspNetCore.Models.ResponseResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCoffee.AspNetCore.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        #region SucceededResult
        protected virtual Response SucceededResult()
        {
            return new Response()
            {
                Code = CustomStatusCode.OK,
                Title = nameof(CustomStatusCode.OK)
            };
        }
        protected virtual Response SucceededResult(object data, string title = nameof(CustomStatusCode.OK))
        {
            return new Response()
            {
                Code = CustomStatusCode.OK,
                Title = title,
                Data = data
            };
        }
        protected virtual Response SucceededResult(object data, string message, string title = nameof(CustomStatusCode.OK))
        {
            return new Response()
            {
                Code = CustomStatusCode.OK,
                Title = title,
                Data = data,
                Message = message
            };
        }

        protected virtual Response PaginationQueryResult(object items, long total)
        {
            return new Response()
            {
                Code = CustomStatusCode.OK,
                Title = nameof(CustomStatusCode.OK),
                Data = new PaginationQueryResult() { Items = items, Total = total }
            };
        }
        #endregion

        #region FailedResult
        protected virtual Response FailedResult(string message, string title = nameof(CustomStatusCode.BadRequest))
        {
            return new Response()
            {
                Code = CustomStatusCode.BadRequest,
                Title = title,
                Message = message
            };
        }
        protected virtual Response FailedResult(object data, string message, string title = nameof(CustomStatusCode.BadRequest))
        {
            return new Response()
            {
                Code = CustomStatusCode.BadRequest,
                Title = title,
                Data = data,
                Message = message
            };
        }
        #endregion

    }
}
