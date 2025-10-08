using gigu_back_end.User.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace gigu_back_end.Shared.Infraestructure.Middlewares;

public class AutheMiddleware
{
    private readonly RequestDelegate _next;

    public AutheMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, IJwtEncryptService jwtEncryptService)
    {
        var allowAnonymous = IsAllowAnonymous(context);

        if (allowAnonymous)
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("token is missing");
            return;
        }

        var user = jwtEncryptService.Decrypt(token);

        context.Items["User"] = user;

        await _next(context);
    }
    private bool IsAllowAnonymous(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint == null) return false;

        var allowAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null;

        if (!allowAnonymous)
        {
            var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (controllerActionDescriptor != null)
            {
                allowAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
                    .Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));
            }
        }

        return allowAnonymous;
    }   
}