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
        
        private ISettingsService Settings;

        public HomeController(IAuthService authService, ISettingsService settings)
        {
            AuthService = authService;
            Settings = settings;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
