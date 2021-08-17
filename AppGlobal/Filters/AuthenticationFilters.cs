using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AppGlobal.Services;
using AppGlobal.Models;
using AppGlobal.Extensions;
using AppGlobal.Commons;
using System.Threading.Tasks;
using AppGlobal.Entities;

namespace AppGlobal.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = (string)context.HttpContext.Items["Token"];
            if (token == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AccessTokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachUserToContext(context, tokenService, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, AccessTokenService tokenService, string token)
        {
            try
            {
                var user = tokenService.ParseJwtToken(token);
                
                context.Items["Token"] = token;

                context.Items["FullName"] = user.FullName;
                context.Items["UserID"] = user.UserID;
                context.Items["Email"] = user.Email;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
