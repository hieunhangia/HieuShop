using System.Reflection;
using Application.Common.Behaviors;
using Application.Features.Identity;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddScoped<IIdentityService, IdentityService>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        builder.Services.AddMappers();
    }
}

public static class DependencyInjectionExtensions
{
    public static void AddMappers(this IServiceCollection services)
    {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                     .Where(t => t is { IsClass: true, IsAbstract: false } && t.Name.EndsWith("Mapper")))
        {
            services.AddSingleton(type);
        }
    }
}