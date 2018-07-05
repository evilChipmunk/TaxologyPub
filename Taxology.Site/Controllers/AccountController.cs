using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Authentication;
using Taxology.Site.Models.AccountViewModels;
using Taxology.Site.Services;

namespace Taxology.Site.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly SignInManager<SiteUser> _signInManager;

        private readonly IShoppingCartService shoppingCartService;
 

        public AccountController(
            UserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager, IShoppingCartService shoppingCartService,
            ILogger<AccountController> logger) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.shoppingCartService = shoppingCartService;
            //  _mailService = mailService;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var anonCart = await shoppingCartService.GetShoppingCart();

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                   // _logger.LogInformation(1, "User logged in.");

                    var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

                    var loggedInCart = await shoppingCartService.GetShoppingCart(user);

                    foreach (var product in anonCart.Products)
                    { 
                        await shoppingCartService.AddProduct(loggedInCart, product);
                    }


                    return RedirectToLocal(returnUrl);
                } 
            }

            ModelState.AddModelError(string.Empty, "Unable to login. Please re-enter your username and password");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

 

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
          //  _logger.LogInformation(4, "User logged out.");
            return RedirectToAction("Index", "Home");
        } 


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<SiteUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}