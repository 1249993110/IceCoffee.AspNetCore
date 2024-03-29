﻿using Microsoft.AspNetCore.Authorization;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors.Security;

namespace IceCoffee.AspNetCore
{
    /// <summary>Generates the Http fallback policy for an operation by reflecting the AuthorizeAttribute attributes.</summary>
    /// <remarks>
    /// 实现所有非匿名方法无需添加 <see cref="AuthorizeAttribute"/> 也会在 Swagger 文档中要求认证授权
    /// </remarks>
    public class AspNetCoreOperationFallbackPolicyProcessor : IOperationProcessor
    {
        private readonly string _name;

        /// <summary>Initializes a new instance of the <see cref="OperationSecurityScopeProcessor"/> class.</summary>
        /// <param name="name">The security definition name.</param>
        public AspNetCoreOperationFallbackPolicyProcessor(string name)
        {
            _name = name;
        }

        /// <summary>Processes the specified method information.</summary>
        /// <param name="context"></param>
        /// <returns>true if the operation should be added to the Swagger specification.</returns>
        public bool Process(OperationProcessorContext context)
        {
            if (context is AspNetCoreOperationProcessorContext aspNetCoreContext)
            {
                var endpointMetadata = aspNetCoreContext.ApiDescription.ActionDescriptor.EndpointMetadata;
                var allowAnonymous = endpointMetadata.OfType<AllowAnonymousAttribute>().Any();
                if (allowAnonymous)
                {
                    return true;
                }

                //var authorizeAttributes = endpointMetadata.OfType<AuthorizeAttribute>().ToList();
                //if (authorizeAttributes.Any() == false)
                //{
                //    return true;
                //}

                var operation = context.OperationDescription.Operation;
                if (operation.Security == null)
                {
                    operation.Security = new List<OpenApiSecurityRequirement>();
                }

                operation.Security.Add(new OpenApiSecurityRequirement()
                {
                    { _name, Enumerable.Empty<string>() }
                });

                return true;
            }

            return false;
        }
    }
}