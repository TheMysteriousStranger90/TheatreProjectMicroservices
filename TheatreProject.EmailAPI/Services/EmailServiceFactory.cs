using Microsoft.Extensions.Options;
using TheatreProject.EmailAPI.Helpers;
using TheatreProject.EmailAPI.Services.Interfaces;

namespace TheatreProject.EmailAPI.Services;

public class EmailServiceFactory : IEmailServiceFactory
{
    private readonly IOptions<MailSettings> _mailSettings;

    public EmailServiceFactory(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings;
    }

    public IEmailService CreateEmailService()
    {
        return new EmailService(_mailSettings);
    }
}