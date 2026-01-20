using Application.Common.Interfaces;
using Domain.Entities.Users;
using FluentEmail.MailKitSmtp;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Identity;
using Infrastructure.Identity.TokenProviders;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<AuditableEntityInterceptor>();

        builder.Services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>());
        });

        builder.Services
            .AddFluentEmail(builder.Configuration["EmailAddress"], builder.Configuration["EmailDisplayName"])
            .AddMailKitSender(new SmtpClientOptions
            {
                Server = builder.Configuration["SmtpServer"],
                Port = int.Parse(builder.Configuration["SmtpPort"]!),
                User = builder.Configuration["SmtpUser"],
                Password = builder.Configuration["SmtpPassword"],
                RequiresAuthentication = true
            });

        builder.Services.AddTransient<IEmailService, EmailService>();

        builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddErrorDescriber<CustomIdentityErrorDescriber>();

        builder.Services.AddTransient<CustomEmailConfirmationTokenProvider>();
        builder.Services.AddTransient<CustomPasswordResetTokenProvider>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
                new TokenProviderDescriptor(typeof(CustomEmailConfirmationTokenProvider)));
            options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

            options.Tokens.ProviderMap.Add("CustomPasswordReset",
                new TokenProviderDescriptor(typeof(CustomPasswordResetTokenProvider)));
            options.Tokens.PasswordResetTokenProvider = "CustomPasswordReset";
        });

        builder.Services.AddAuthorization();
    }
}