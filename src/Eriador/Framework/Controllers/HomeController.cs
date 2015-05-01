using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Framework.Services.Auth;
using Microsoft.AspNet.Mvc;

namespace Eriador.Controllers
{
    public class HomeController : Controller
    {
        private IAuthService AuthService;

        public HomeController(IAuthService authService)
        {
            AuthService = authService;
        }
        public IActionResult Index()
        {
            AuthService.CurrentPermissions();
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

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
