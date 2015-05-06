using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Framework.Services.Auth;
using Eriador.Framework.Services.Settings;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Eriador.Framework.Security;

namespace Eriador.Controllers
{
    public class HomeController : Controller
    {
        private IAuthService AuthService;

        
        public IOptions<AppSettings> fds { get; set; }

        private ISettingsService Settings;

        public HomeController(IAuthService authService, ISettingsService settings)
        {
            AuthService = authService;
            Settings = settings;
        }
        public IActionResult Index()
        {
            AuthService.CurrentPermissions();
            //fds.Options.Theme = "huhuhuhu";
            //Settings.Set("puff", "piff");
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //var dsf = fds.Options.Theme;
            var asd = Settings.Get("puff");
            return View();
        }

        [PermissionAuthorize(Permission = "proba", Policy = "Permission")]
        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
