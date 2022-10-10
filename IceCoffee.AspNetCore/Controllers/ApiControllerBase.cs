using IceCoffee.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace IceCoffee.AspNetCore.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected virtual UserInfo UserInfo => HttpContext.RequestServices.GetService<UserInfo>() ?? new UserInfo(HttpContext.User.Claims);

        #region SucceededResult

        protected virtual Response SucceededResult()
        {
            return new Response()
            {
                Status = HttpStatus.OK
            };
        }

        protected virtual Response<TData> SucceededResult<TData>()
        {
            return new Response<TData>()
            {
                Status = HttpStatus.OK
            };
        }

        protected virtual Response<TData> SucceededResult<TData>(TData data)
        {
            return new Response<TData>()
            {
                Status = HttpStatus.OK,
                Data = data
            };
        }

        protected virtual Response<PaginationQueryResult<TItems>> PaginationQueryResult<TItems>(IEnumerable<TItems> items, int total)
        {
            return new Response<PaginationQueryResult<TItems>>()
            {
                Status = HttpStatus.OK,
                Data = new PaginationQueryResult<TItems>() 
                { 
                    Items = items, 
                    Total = total 
                }
            };
        }

        protected virtual Response<PaginationQueryResult<TItems>> PaginationQueryResult<TItems>(PaginationQueryResult<TItems> paginationQueryResult)
        {
            return new Response<PaginationQueryResult<TItems>>()
            {
                Status = HttpStatus.OK,
                Data = paginationQueryResult
            };
        }

        #endregion SucceededResult

        #region FailedResult

        protected virtual Response FailedResult(string message)
        {
            return new Response()
            {
                Status = HttpStatus.BadRequest,
                Error = new Error() 
                {
                    Message = message
                }
            };
        }

        protected virtual Response FailedResult(string message, string[] details)
        {
            return new Response()
            {
                Status = HttpStatus.BadRequest,
                Error = new Error()
                {
                    Message = message,
                    Details = details
                }
            };
        }

        #endregion FailedResult

        #region ForbiddenResult
        protected virtual Response ForbiddenResult(string? message = nameof(HttpStatus.Forbidden))
        {
            return new Response()
            {
                Status = HttpStatus.Forbidden,
                Error = new Error()
                {
                    Message = message
                }
            };
        }
        #endregion ForbiddenResult
    }
}