using System.Reflection;
using Application.Features.Brands;
using Application.Features.Categories;
using Application.Features.Identity;
using Application.Features.Products;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IBrandService, BrandService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
    }
}