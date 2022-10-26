using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PERMISSIONSFW.Filters;

namespace PERMISSIONSFW
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPermissions(this IServiceCollection services)
        {

            #region Custom Authorization
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            #endregion

            return services;
        }
    }
}
