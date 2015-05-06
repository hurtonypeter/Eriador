using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Framework.Services.Auth;
using Eriador.Models.Data;
using Eriador.Models.Data.Entity;
using Microsoft.AspNet.Mvc;

namespace Eriador.Framework.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly IAuthService AuthService;

        public MenuViewComponent(ApplicationDbContext context, IAuthService authService)
        {
            db = context;
            AuthService = authService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mainmenus = await db.MenuItems.Include(m => m.Children).Include(m => m.Permission).Where(m => m.Parent == null).ToListAsync();
            var model = new List<MenuItem>();

            foreach (var item in mainmenus)
            {
                if (item.Children != null && item.Children.Any(c => AuthService.HasPermission(c.Permission.MachineReadableName)))
                {
                    var tmp = item;
                    List<MenuItem> todelete = new List<MenuItem>();
                    foreach (var child in item.Children)
                    {
                        if (!AuthService.HasPermission(child.Permission.MachineReadableName))
                        {
                            todelete.Add(child);
                        }
                    }
                    todelete.ForEach(t => tmp.Children.Remove(t));
                    model.Add(tmp);
                }
            }

            return View(model);
        }
    }
}
