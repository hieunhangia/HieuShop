using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Controllers.ActionFilterAttributes;

public class RequireConfirmedEmailAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity is not { IsAuthenticated: true })
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
        var currentUser = await userManager.GetUserAsync(context.HttpContext.User);

        if (currentUser == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!currentUser.EmailConfirmed)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = "Email chưa được xác nhận",
                Detail = "Bạn phải xác nhận email để thực hiện hành động này.",
                Status = StatusCodes.Status403Forbidden
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        await next();
    }
}