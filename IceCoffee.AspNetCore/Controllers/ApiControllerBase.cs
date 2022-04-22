using IceCoffee.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

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

        protected virtual Response SucceededResult(object data)
        {
            return new Response()
            {
                Code = CustomStatusCode.OK,
                Title = nameof(CustomStatusCode.OK),
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

        protected virtual Response PaginationQueryResult(object items, uint total)
        {
            return new Response()
            {
                Code = CustomStatusCode.OK,
                Title = nameof(CustomStatusCode.OK),
                Data = new PaginationQueryResult() { Items = items, Total = total }
            };
        }

        #endregion SucceededResult

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

        #endregion FailedResult

        #region ForbiddenResult
        protected virtual Response ForbiddenResult(string? message = null, string title = nameof(CustomStatusCode.Forbidden))
        {
            return new Response()
            {
                Code = CustomStatusCode.Forbidden,
                Title = title,
                Message = message
            };
        }
        #endregion ForbiddenResult
    }
}