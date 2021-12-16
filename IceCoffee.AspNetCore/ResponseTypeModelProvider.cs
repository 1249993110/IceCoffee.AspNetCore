using IceCoffee.AspNetCore.Models;
using IceCoffee.AspNetCore.Models.Primitives;
using IceCoffee.AspNetCore.Models.ResponseResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCoffee.AspNetCore
{
    public class ResponseTypeModelProvider : IApplicationModelProvider
    {
        public int Order => 1024;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                bool controllerAllowAnonymous = controller.Attributes.Any(p => p is AllowAnonymousAttribute);
                foreach (ActionModel action in controller.Actions)
                {
                    bool existStatus200 = false;
                    foreach (var filter in action.Filters)
                    {
                        if (filter is ProducesResponseTypeAttribute producesResponseTypeAttribute)
                        {
                            var statusCode = producesResponseTypeAttribute.StatusCode;
                            if (statusCode == StatusCodes.Status200OK)
                            {
                                existStatus200 = true;
                                break;
                            }
                        }
                    }

                    if (existStatus200 == false)
                    {
                        Type returnType = action.ActionMethod.ReturnType;
                        // If actions type are Task<RespResult<ReturnType>>
                        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            returnType = returnType.GenericTypeArguments[0];
                        }

                        if (returnType == typeof(IResponse))
                        {
                            returnType = typeof(Response);
                        }

                        action.Filters.Add(new ProducesResponseTypeAttribute(returnType, StatusCodes.Status200OK));
                    }

                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(Response), StatusCodes.Status400BadRequest));

                    if (controllerAllowAnonymous == false && action.Attributes.Any(p => p is AllowAnonymousAttribute) == false)
                    {
                        action.Filters.Add(new ProducesResponseTypeAttribute(typeof(int), StatusCodes.Status401Unauthorized));
                        action.Filters.Add(new ProducesResponseTypeAttribute(typeof(int), StatusCodes.Status403Forbidden));
                    }

                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(Response<InternalServerErrorResult>), StatusCodes.Status500InternalServerError));
                }
            }
        }
    }
}
