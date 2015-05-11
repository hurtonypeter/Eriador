using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Internal;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using Microsoft.Framework.DependencyInjection;

namespace Eriador.Framework.Security
{
    public class PermissionAuthorizeFilter : IAsyncAuthorizationFilter
    {

        public async Task OnAuthorizationAsync(Microsoft.AspNet.Mvc.AuthorizationContext context)
        {

            //if (Policy.ActiveAuthenticationSchemes != null && Policy.ActiveAuthenticationSchemes.Any())
            //{
            //    var newPrincipal = new ClaimsPrincipal();
            //    foreach (var scheme in Policy.ActiveAuthenticationSchemes)
            //    {
            //        var result = (await context.HttpContext.Authentication.AuthenticateAsync(scheme))?.Principal;
            //        if (result != null)
            //        {
            //            newPrincipal.AddIdentities(result.Identities);
            //        }
            //    }
            //    context.HttpContext.User = newPrincipal;
            //}

            if (context.Filters.Any(item => item is IAllowAnonymous))
            {
                return;
            }
        }
    }
}
