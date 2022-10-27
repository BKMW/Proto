using Application.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
          
                #region Use DB

                services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
                services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("testConnection")));

            #endregion

            services.AddScoped<ITokenService, TokenService>();
            // second way to get claims
                services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddTransient<IMailingService, MailingService>();


            return services;
        }
    }
}