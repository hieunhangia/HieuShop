using API.Infrastructure;
using API.Services;
using Application.Common.Interfaces;

namespace API;

public static class DependencyInjection
{
    public static void AddApiServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddProblemDetails();

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowClient",
                policy => policy.WithOrigins(builder.Configuration["ClientUrl"]!)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });
    }
}