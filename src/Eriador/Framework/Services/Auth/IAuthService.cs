using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Models.Data.Entity;

namespace Eriador.Framework.Services.Auth
{
    public interface IAuthService
    {
        int CurrentUserId();
        string CurrentUserName();
        User CurrentUser();
        bool IsAuthenticated();
        IEnumerable<Permission> CurrentPermissions();
        IEnumerable<string> CurrentPermissionNames();
        bool HasPermission(string perm);
    }
}
