using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Framework.Modules.People.Models;
using Eriador.Framework.Security;
using Eriador.Framework.Services.UserService;
using Eriador.Models.Data;
using Eriador.Models.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;

namespace Eriador.Framework.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext db;
        
        private readonly IUserService UserService;

        private readonly RoleManager<Role> RoleManager;

        private readonly UserManager<User> UserManager;

        /// <summary>
        /// DI kontruktor
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="roleMgr"></param>
        /// <param name="userMgr"></param>
        /// <param name="context"></param>
        public PeopleController(IUserService userService, RoleManager<Role> roleMgr, UserManager<User> userMgr, ApplicationDbContext context)
        {
            UserService = userService;
            UserManager = userMgr;
            RoleManager = roleMgr;
            db = context;
        }

        #region Felhasználók kezelése
        /// <summary>
        /// Listázza a létező felhasználókat
        /// </summary>
        /// <returns></returns>
        //[PermissionAuthorize(Permission = "ManageUsers")]
        public async Task<IActionResult> Index()
        {
            return View(await UserService.GetAllUser());
        }

        /// <summary>
        /// Létrehozza a formot felhasználó létrehozásához vagy szerkesztéséhez.
        /// </summary>
        /// <param name="id">A felhasználó azonosítója, ha nincs megadva, akkor új felhasználót kell csinálni</param>
        /// <returns></returns>
        //[PermissionAuthorize(Permission = "ManageUsers")]
        public async Task<IActionResult> EditUser(int? id)
        {
            var model = new EditUserViewModel();
            var roles = await db.Roles.Where(r => r.Name.ToLower() != "anonymous" && r.Name.ToLower() != "authenticated").ToListAsync();

            if (id.HasValue)
            {
                var user = await UserService.GetUserById(id.Value);
                if (user == null)
                {
                    return HttpNotFound();
                }
                model.User = user;
                foreach (var role in roles)
                {
                    model.UserRoles.Add(new RoleCheckBox
                    {
                        Name = role.Name,
                        Checked = await UserManager.IsInRoleAsync(model.User, role.Name)
                    });
                }

                return View(model);
            }
            else
            {
                model.User = new User();
                model.UserRoles = roles.Select(r => new RoleCheckBox { Name = r.Name, Checked = false }).ToList();

                return View(model);
            }
        }

        /// <summary>
        /// Felhasználó hozzáadása vagy szerkesztése, jelszó beállítása, szerepkörökhöz hozzáadás
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Password">jelszó, ha új felhasználó, akkor kötelező</param>
        /// <param name="PasswordConfirmation">jelszó megerősítés</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionAuthorize(Permission = "ManageUsers")]
        public async Task<IActionResult> EditUser(EditUserViewModel model, string Password, string PasswordConfirmation)
        {
            if ((!string.IsNullOrWhiteSpace(Password) || !string.IsNullOrWhiteSpace(PasswordConfirmation)) && Password != PasswordConfirmation)
            {
                ModelState.AddModelError("Password", "A megadott jelszó nem egyezik!");
            }
            if (model.User.Id == default(int) && string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("Password", "Jelszó megadása kötelező!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.User.Id == default(int))
                    {
                        //felhasználó hozzáadása
                        var result = await UserManager.CreateAsync(model.User, Password);
                        if (!result.Succeeded)
                        {
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                            return View(model);
                        }
                        foreach (var role in model.UserRoles.Where(r => r.Checked))
                        {
                            await UserManager.AddToRoleAsync(model.User, role.Name);
                        }
                    }
                    else
                    {
                        //felhasználó szerkesztése
                        var result = await UserManager.UpdateAsync(model.User);
                        if (!result.Succeeded)
                        {
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                            return View(model);
                        }
                        foreach (var role in model.UserRoles)
                        {
                            if (role.Checked)
                            {
                                await UserManager.AddToRoleAsync(model.User, role.Name);
                            }
                            else
                            {
                                await UserManager.RemoveFromRoleAsync(model.User, role.Name);
                            }
                        }
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Entity Framework 7 beta bug, a threadek összeakadnak az aszinkron update-eknél");
                    return View(model);
                }
                
            }

            return View(model);
        }

        /// <summary>
        /// Rendereli a felhasználó törlésének megerősítéséhez a formot
        /// </summary>
        /// <param name="id">felhasználó azonosítója</param>
        /// <returns></returns>
        //[PermissionAuthorize(Permission = "ManageUsers")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var model = await UserService.GetUserById(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        /// <summary>
        /// Törli a megkapott felhasználót
        /// </summary>
        /// <param name="user">A törlendő felhasználó</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionAuthorize(Permission = "ManageUsers")]
        public async Task<IActionResult> DeleteUser(User user)
        {

            try
            {
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(user);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Entity Framework 7 beta hiba.");
                return View(user);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Szerepkörök kezelése

        /// <summary>
        /// Listázza a rendszerben lévő szerepköröket
        /// </summary>
        /// <returns></returns>
        //[PermissionAuthorize(Permission = "ManageRoles")]
        public async Task<IActionResult> Roles()
        {
            return View(await db.Roles.Where(r => r.Name.ToLower() != "anonymous" && r.Name.ToLower() != "authenticated").ToListAsync());
        }

        /// <summary>
        /// Rendereli a szerepkör szerkesztéséhez szükséges formot
        /// </summary>
        /// <param name="id">szerepkör azonosítója, null, ha új szerepkört kell létrehozni</param>
        /// <returns></returns>
        //[PermissionAuthorize(Permission = "ManageRoles")]
        public async Task<IActionResult> EditRole(int? id)
        {
            if (id.HasValue)
            {
                //role szerkesztése
                var role = await db.Roles.Where(r => r.Name.ToLower() != "anonymous" && r.Name.ToLower() != "authenticated").SingleOrDefaultAsync(r => r.Id == id.Value);
                if (role == null)
                {
                    return HttpNotFound();
                }

                return View(role);
            }
            else
            {
                //új role létrehozása
                return View(new Role());
            }
        }

        /// <summary>
        /// Menti a szerepkört
        /// </summary>
        /// <param name="role">szerkesztendő role</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionAuthorize(Permission = "ManageRoles")]
        public async Task<IActionResult> EditRole(Role role)
        {
            try
            {
                if (role.Id != default(int))
                {
                    //role szerkesztése
                    var result = await RoleManager.UpdateAsync(role);
                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                            return View(role);
                        }
                    }
                }
                else
                {
                    //új role létrehozása
                    var result = await RoleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                            return View(role);
                        }
                    }
                }

                return RedirectToAction("Roles");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "EF 7 beta hiba.");
                return View(role);
            }
        }

        /// <summary>
        /// Rendereli a szerepkört törlésének formját
        /// </summary>
        /// <param name="id">szerepkör azonosítója</param>
        /// <returns></returns>
        //[PermissionAuthorize(Permission = "ManageRoles")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await db.Roles.Where(r => r.Name.ToLower() != "anonymous" && r.Name.ToLower() != "authenticated").SingleOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        /// <summary>
        /// Törli a visszakapott szerepkört
        /// </summary>
        /// <param name="role">törlendő szerepkör</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionAuthorize(Permission = "ManageRoles")]
        public async Task<IActionResult> DeleteRole(Role role)
        {
            try
            {
                var result = await RoleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(role);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(role);
            }

            return RedirectToAction("Roles");
        }

        #endregion

        #region Jogosultságok kezelése

        /// <summary>
        /// Jogosultások kezelésének formját jeleníti meg
        /// </summary>
        /// <returns></returns>
        //[PermissionAuthorize(Permission = "ManagePermissions")]
        public async Task<IActionResult> Permissions()
        {
            var model = new PermissionsTableViewModel
            {
                Roles = await db.Roles.Include(per => per.Permissions).ToListAsync()
            };

            //nem akar működni, pedig EF6 alatt jó
            //var modules = await db.Permissions.Include(p => p.Roles.Select(r => r.Role)).Include(p => p.Module).GroupBy(p => p.Module).ToListAsync();
            //var modules = await (from permission in db.Permissions
            //              join module in db.Modules on permission.Id equals module.Id
            //              join rolepermission in db.RolePermission on permission.Id equals rolepermission.PermissionId
            //              group permission by module.Name).ToListAsync();

            //állítsuk össze saját magunk a csoportosított lekérdezést, így ha megjavul az EF7, nem kell
            //újraírni a ViewModelt feltöltő jó algoritmust, csak a fenti kikommentezett részt használni ehelyett
            var m = await db.Modules.ToListAsync();
            var p = await db.Permissions.Include(per => per.Module).Include(per => per.Roles).ToListAsync();
            List<IGrouping<Module, Permission>> modules = new List<IGrouping<Module, Permission>>();
            foreach (var item in m)
            {
                modules.Add(new Grouping<Module, Permission>(item, p.Where(per => per.Module.Id == item.Id).ToList()));
            }

            foreach (var module in modules)
            {
                var mod = new ModuleWithPermissions
                {
                    ModuleName = module.Key.Name
                };
                foreach (var permission in module)
                {
                    var perm = new PermissionLine
                    {
                        PermissionId = permission.Id,
                        PermissionName = permission.Name
                    };
                    foreach (var role in model.Roles)
                    {
                        if (permission.Roles.Select(r => r.RoleId).ToList().Contains(role.Id))
                        {
                            perm.Roles.Add(true);
                        }
                        else
                        {
                            perm.Roles.Add(false);
                        }
                    }
                    mod.Permissions.Add(perm);
                }
                model.Modules.Add(mod);
            }

            return View(model);
        }

        /// <summary>
        /// Menti az adatbázisba a beállított jogosultásokat a szerepkörökhöz
        /// </summary>
        /// <param name="model">Jogosultság táblázat modellje</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionAuthorize(Permission = "ManagePermissions")]
        public async Task<IActionResult> Permissions(PermissionsTableViewModel model)
        {
            try
            {
                var r = await db.Roles.Include(per => per.Permissions).ToListAsync();
                var p = await db.Permissions.Include(per => per.Module).Include(per => per.Roles).ToListAsync();

                foreach (var module in model.Modules)
                {
                    foreach (var permission in module.Permissions)
                    {
                        int i = 0;
                        foreach (var role in permission.Roles)
                        {
                            var perm = p.Single(s => s.Id == permission.PermissionId).Roles;
                            var currentrole = r[i];
                            var intersect = currentrole.Permissions.Intersect(perm);
                            if (role && !intersect.Any())
                            {
                                //ki van pipálva, de nincs összepárosítva a db-ben -> adjuk hozzá
                                db.RolePermission.Add(new RolePermission { RoleId = currentrole.Id, PermissionId = permission.PermissionId });
                            }
                            if (!role && intersect.Any())
                            {
                                //nincs kipipálva, de össze van párosítva a db-ben -> töröljük
                                var rp = await db.RolePermission.SingleAsync(s => s.PermissionId == permission.PermissionId && s.RoleId == currentrole.Id);
                                db.RolePermission.Remove(rp);
                            }
                            i++;
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }
        #endregion
    }

    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {

        readonly List<TElement> elements;

        public Grouping(IGrouping<TKey, TElement> grouping)
        {
            if (grouping == null)
                throw new ArgumentNullException("grouping");
            Key = grouping.Key;
            elements = grouping.ToList();
        }

        public Grouping(TKey key, IEnumerable<TElement> el)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (el == null)
                throw new ArgumentNullException("el");
            Key = key;
            elements = el.ToList();
        }

        public TKey Key { get; private set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    }
}
