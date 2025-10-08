using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace gigu_back_end.Shared.Infraestructure.Attribute;

public class CustomAuthorizeAttribute : System.Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as User.Domain.Models.Entities.User; //User1 role mkt

        if (user == null || !_roles[0].Contains(user.Role)) //posible modificacion --> mensaje para token invalido o caducado y mensaje para falta de autorizacion
        {
            context.Result = new ForbidResult();
        }
    }

}