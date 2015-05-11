using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Framework.Models;
using Eriador.Framework.Security;
using Eriador.Framework.Services.Auth;
using Eriador.Framework.Services.Settings;
using Eriador.Models.Data;
using Eriador.Modules.News.Models;
using Eriador.Modules.News.Models.Data.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Eriador.Modules.News.Controllers
{
    public class NewsController : Controller
    {
        private readonly ISettingsService SettingService;

        private readonly IAuthService AuthService;

        private readonly ApplicationDbContext db;

        public NewsController(ISettingsService settingService, IAuthService authService, ApplicationDbContext context)
        {
            SettingService = settingService;
            AuthService = authService;
            db = context;
        }

        /// <summary>
        /// Listázza a rendszerben lévő hírleveleket
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize(Permission = "ListSentMails")]
        public async Task<IActionResult> Index()
        {
            if (AuthService.HasPermission("ListAllMails"))
            {
                return View(await db.NewsPapers.Include(s => s.Editor).ToListAsync());
            }

            return View(await db.NewsPapers.Include(s => s.Editor).Where(s => s.Status == Models.Data.Entity.NewsPaperStatus.Sent).ToListAsync());
        }

        /// <summary>
        /// Letölti a szerkeszteni kívánt hírlevelet, ha id nincs megadva, újat hoz létre
        /// </summary>
        /// <param name="id">szerkesztendő hírlevél</param>
        /// <returns></returns>
        [PermissionAuthorize(Permission = "SendMail")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var news = await db.NewsPapers.Include(s => s.Editor).Include(s => s.NewsItems).SingleOrDefaultAsync(s => s.Id == id);
                if (news == null)
                {
                    return HttpNotFound();
                }
                var model = new NewsPaperViewModel
                {
                    Id = news.Id,
                    Editor = news.Editor.UserName,
                    RPublisher = news.RPublisher,
                    REditor = news.REditor,
                    Title = news.Title,
                    News = news.NewsItems.Select(s => new NewsPaperItemViewModel
                    {
                        Title = s.Title,
                        Link = s.Link,
                        Body = s.Body
                    }).ToList()
                };
                return View(model);
            }
            else
            {
                var model = new NewsPaperViewModel
                {
                    Editor = AuthService.CurrentUser().FullName,
                    RPublisher = SettingService.Get("News:rpublisher"),
                    REditor = SettingService.Get("News:reditor")
                };
                return View(model);
            }
        }

        /// <summary>
        /// Menti az adatbázisba a visszakapott hírlevelet
        /// </summary>
        /// <param name="model">mentendő hírlevél</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(Permission = "SendMail")]
        public async Task<IActionResult> Edit([FromBody] NewsPaperViewModel model)
        {

            try
            {
                if (model.Id == default(int))
                {
                    var news = new NewsPaper
                    {
                        Editor = await db.Users.SingleAsync(s => s.Id == AuthService.CurrentUserId()),
                        RPublisher = model.RPublisher,
                        REditor = model.REditor,
                        Created = DateTime.Now,
                        LastEdited = DateTime.Now,
                        Title = string.IsNullOrWhiteSpace(model.Title) ? string.Join(", ", model.News.Select(s => s.Title)) : model.Title,
                        Status = NewsPaperStatus.Draft,
                        NewsItems = model.News.Select(s => new NewsItem
                        {
                            Title = s.Title,
                            Link = s.Link,
                            Body = s.Body
                        }).ToList()
                    };
                    db.NewsPapers.Add(news);
                    db.NewsItems.AddRange(news.NewsItems);
                    await db.SaveChangesAsync();

                    return Json(new BaseJsonModel { redirectToUrl = Url.Action("Index", "News") });
                }
                else
                {
                    var news = await db.NewsPapers.Include(s => s.Editor).Include(s => s.NewsItems).SingleOrDefaultAsync(s => s.Id == model.Id);
                    news.Title = model.Title;
                    news.Editor = await db.Users.SingleAsync(s => s.Id == AuthService.CurrentUserId());
                    news.RPublisher = model.RPublisher;
                    news.REditor = model.REditor;
                    news.LastEdited = DateTime.Now;
                    List<NewsItem> todelete = new List<NewsItem>();
                    foreach (var item in news.NewsItems)
                    {
                        var current = model.News.SingleOrDefault(s => s.Id == item.Id);
                        if (current == null)
                        {
                            todelete.Add(item);
                        }
                        else
                        {
                            item.Title = current.Title;
                            item.Link = current.Link;
                            item.Body = current.Body;
                        }
                    }
                    todelete.ForEach(s => news.NewsItems.Remove(s));
                    foreach (var item in model.News.Where(s => s.Id == default(int)))
                    {
                        news.NewsItems.Add(new NewsItem
                        {
                            Title = item.Title,
                            Link = item.Link,
                            Body = item.Body
                        });
                    }

                    await db.SaveChangesAsync();

                    return Json(new BaseJsonModel { redirectToUrl = Url.Action("Index", "News") });
                }
            }
            catch (Exception e)
            {
                return Json(new BaseJsonModel(e.Message));
            }
            
        }

        /// <summary>
        /// Töröltre állítja a megadott azonosítójú levelet
        /// </summary>
        /// <param name="id">törlendő levél azonosítója</param>
        /// <returns></returns>
        [PermissionAuthorize(Permission = "DeleteMail")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await db.NewsPapers.SingleOrDefaultAsync(s => s.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission = "DeleteMail")]
        public async Task<IActionResult> Delete(NewsPaper model)
        {
            try
            {
                model = await db.NewsPapers.SingleOrDefaultAsync(s => s.Id == model.Id);
                model.Status = NewsPaperStatus.Deleted;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }

        [PermissionAuthorize(Permission = "SendMail")]
        public async Task<IActionResult> Send(int id)
        {
            var model = await db.NewsPapers.SingleOrDefaultAsync(s => s.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize(Permission = "SendMail")]
        public async Task<IActionResult> Send(NewsPaper model)
        {
            try
            {
                model = await db.NewsPapers.SingleOrDefaultAsync(s => s.Id == model.Id);
                //itt kéne postallal elküldeni a levelet
                //dynamic email = new Email("MailTemplate");
                //email.Model = model;
                //email.Send();
                throw new Exception("A Postal sajnos még nem kompatibilis az új smtp layerrel.");

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }

        /// <summary>
        /// betölti a beállítások formot
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize(Permission = "MailSettings")]
        public async Task<IActionResult> Settings()
        {
            using (var fs = new FileStream(@"C:\Users\Pet\Documents\visual studio 2015\Projects\Eriador\src\Eriador\Views\News\MailTemplate.cshtml", FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    ViewBag.MailTemplate = await sr.ReadToEndAsync();
                }
            }

            var model = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("News:rpublisher", SettingService.Get("News:rpublisher")),
                new KeyValuePair<string, string>("News:reditor", SettingService.Get("News:reditor")),
            };
            
            return View(model);
        }

        /// <summary>
        /// Menti a felpostolt beállítást
        /// </summary>
        /// <param name="key">beállítás kulcsa</param>
        /// <param name="value">beállítás új értéke</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(Permission = "MailSettings")]
        public async Task<IActionResult> SaveSetting(string key, string value)
        {
            try
            {
                SettingService.Set(key, value);
                return Json(new { saved = true });
            }
            catch (Exception)
            {
                return Json(new { saved = false });
            }

        }
        
        /// <summary>
        /// Elmenti a felpostolt template fájlt
        /// </summary>
        /// <param name="template">hívlevél template fájlja</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize(Permission = "MailSettings")]
        public async Task<IActionResult> SaveTemplate(string template)
        {
            try
            {
                using (var fs = new FileStream(@"C:\Users\Pet\Documents\visual studio 2015\Projects\Eriador\src\Eriador\Views\News\MailTemplate.cshtml", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        await sw.WriteAsync(template);
                    }
                }

                return Json(new { saved = true });
            }
            catch (Exception)
            {
                return Json(new { saved = false });
            }
        }
    }
    
}
