using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Eriador.Framework.Models;
using Microsoft.AspNet.Identity;
using Eriador.Models.Data.Entity;
using Eriador.Framework.Services.Auth;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Eriador.Framework.Controllers
{
    [Route("api/system")]
    public class MobileApiController : Controller
    {
        private UserManager<User> UserManager;

        private SignInManager<User> SignInManager;

        private IAuthService AuthService;

        public MobileApiController(UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IAuthService authService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            AuthService = authService;
        }


        // GET: api/values
        [HttpGet]
        [Route("permissions")]
        public IEnumerable<string> Permissions()
        {
            return AuthService.CurrentPermissionNames();
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login( MobileLoginRequestModel model)
        {
            try
            {
                var result = await SignInManager.PasswordSignInAsync(model.username, model.password, model.rememberme, shouldLockout: false);
                if (result.Succeeded)
                {
                    return Json(new MobileLoginResponseModel
                    {
                        loginSuccess = true
                    });
                }
                if (result.RequiresTwoFactor)
                {
                    return Json(new MobileLoginResponseModel
                    {
                        loginSuccess = false,
                        errorMessage = "Requires two-factor authentication."
                    });
                }
                if (result.IsLockedOut)
                {
                    return Json(new MobileLoginResponseModel
                    {
                        loginSuccess = false,
                        errorMessage = "User is locked out."
                    });
                }
                else
                {
                    return Json(new MobileLoginResponseModel
                    {
                        loginSuccess = false,
                        errorMessage = "Invalid login attempt."
                    });
                }
            }
            catch (Exception e)
            {
                return Json(new MobileLoginResponseModel
                {
                    loginSuccess = false,
                    errorMessage = e.Message
                });
            }
        }
    }
}
