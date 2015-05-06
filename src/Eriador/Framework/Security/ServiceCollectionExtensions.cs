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
            services.Remove(ServiceDescriptor.Transient<IAuthorizationHandler, ClaimsAuthorizationHandler>());
            services.Remove(ServiceDescriptor.Transient<IAuthorizationHandler, DenyAnonymousAuthorizationHandler>());
            services.Remove(ServiceDescriptor.Transient<IAuthorizationHandler, PassThroughAuthorizationHandler>());

            services.Add(ServiceDescriptor.Transient<IAuthorizationService, PermissionAuthorizationService>());
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            //services.Replace(ServiceDescriptor.Transient<IAuthorizationService, PermissionAuthorizationService>());
            //services.Replace(ServiceDescriptor.Transient<IAuthorizationHandler, PermissionAuthorizationHandler>());


            return services;
        }
    }
}
