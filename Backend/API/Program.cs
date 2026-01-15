using Application.Interfaces;
using Domain.Entities.Users;
using FluentEmail.MailKitSmtp;
using Infrastructure.Data;
using Infrastructure.Identity.TokenProviders;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddFluentEmail(builder.Configuration["EmailAddress"], builder.Configuration["EmailDisplayName"])
    .AddMailKitSender(new SmtpClientOptions
    {
        Server = builder.Configuration["SmtpServer"],
        Port = int.Parse(builder.Configuration["SmtpPort"]!),
        User = builder.Configuration["SmtpUser"],
        Password = builder.Configuration["SmtpPassword"],
        RequiresAuthentication = true
    });

builder.Services.AddTransient<IEmailService, EmailService>();

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

builder.Services.AddTransient<CustomEmailConfirmationTokenProvider>();
builder.Services.AddTransient<CustomPasswordResetTokenProvider>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient",
        policy => policy.WithOrigins(builder.Configuration["ClientUrl"]!)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

await SeedData.InitializeAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("AllowClient");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();