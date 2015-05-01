using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Models.Data;
using Microsoft.AspNet.Mvc;

namespace Eriador.Framework.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public MenuViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mainmenus = await db.MenuItems.Include(m => m.Children).Where(m => m.Parent == null).ToListAsync();

            return View(mainmenus);
        }
    }
}
