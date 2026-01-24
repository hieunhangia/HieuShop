using System.Reflection;
using Application.Common.Behaviors;
using Application.Features.Brands;
using Application.Features.Categories;
using Application.Features.Identity;
using Application.Features.Products;
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

        builder.Services.AddSingleton<ProductMapper>();
        builder.Services.AddSingleton<CategoryMapper>();
        builder.Services.AddSingleton<BrandMapper>();
    }
}