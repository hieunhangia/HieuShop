using System.Reflection;
using Application.Features.Identity;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IIdentityService, IdentityService>();

        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}