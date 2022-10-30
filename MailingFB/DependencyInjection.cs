using MailingFB.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailingFB
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration configuration)
        {

            #region Custom Authorization
         //   services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            services.AddTransient<IMailingService, MailingService>();

            #endregion

            return services;
        }
    }
}
