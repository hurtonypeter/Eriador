using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.Framework.DependencyInjection;

namespace Eriador.Framework.Security
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
        {
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Transient<IAuthorizationService, PermissionAuthorizationService>());
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return services;
        }
    }
}
