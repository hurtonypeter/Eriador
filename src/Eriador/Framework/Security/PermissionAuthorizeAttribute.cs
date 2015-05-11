using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Framework.Services.Auth;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Framework.Internal;

namespace Eriador.Framework.Security
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAuthorizeAttribute : AuthorizationFilterAttribute
    {
        //[Activate]
        //public IAuthService AuthService { get; set; }

        public string Permission { get; set; }

        public override void OnAuthorization(Microsoft.AspNet.Mvc.AuthorizationContext context)
        {
            if (!AuthService.HasPermission(Permission, context.HttpContext.User))
                Fail(context);
        }
        
        public override Task OnAuthorizationAsync(Microsoft.AspNet.Mvc.AuthorizationContext context)
        {
            if (!AuthService.HasPermission(Permission, context.HttpContext.User))
                Fail(context);
            return Task.FromResult(0);
        }

    }
}
