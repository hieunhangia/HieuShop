using Application.Interfaces;
using FluentEmail.Core;

namespace Infrastructure.Services;

public class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var response = await fluentEmail
            .To(toEmail)
            .Subject(subject)
            .Body(message)
            .SendAsync();

        if (!response.Successful)
        {
            throw new Exception("An error occurred while sending the email.");
        }
    }
}