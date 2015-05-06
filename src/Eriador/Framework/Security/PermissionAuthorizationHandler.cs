using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Framework.Services.Auth;
using Microsoft.AspNet.Authorization;

namespace Eriador.Framework.Security
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirements>
    {
        //[Microsoft.AspNet.Mvc.Activate]
        //public IAuthService AuthService { get; set; }

        public override void Handle(AuthorizationContext context, PermissionAuthorizationRequirements requirement)
        {
            if (AuthService.HasPermission(requirement.Permission, context.User))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        public override Task HandleAsync(AuthorizationContext context)
        {
            foreach (var req in context.Requirements.OfType<PermissionAuthorizationRequirements>())
            {
                Handle(context, req);
            }
            return Task.FromResult(0);
        }
    }
}
