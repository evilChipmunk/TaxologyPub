
using System.Threading.Tasks;
using AutoMapper;
using Customer.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Service;
using Taxology.Site.Models;
using Taxology.Site.Services;
using Taxology.Site.ViewComponents;

namespace Taxology.Site.Controllers
{
    public class RegisterController : BaseWebController
    { 
        private readonly IMapper mapper;
        private readonly ICustomerRestService customerService; 
        private readonly SignInManager<SiteUser> signInManager;
        private readonly AnoymousIdHandler anonHandler;
        private readonly UserManager<SiteUser> userManager;

        public RegisterController(
            UserManager<SiteUser> userManager,  IMapper mapper, ICustomerRestService customerService,
              SignInManager<SiteUser> signInManager, AnoymousIdHandler anonHandler) 
            : base(userManager)
        { 
            this.mapper = mapper;
            this.customerService = customerService; 
            this.signInManager = signInManager;
            this.anonHandler = anonHandler;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(new RegisterModel());
        }

        [HttpPost(Name = "Register")]
        [ValidateModel]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            //TODO add saga -- not needed here unless cart creation is moved here
            var registerDTO = mapper.Map<RegisteredUserDTO>(model);
            registerDTO.AnonymousId = anonHandler.GetAnonId();
             
            var customer = await customerService.Create(registerDTO);

            if (ModelState.IsValid)
            {
                var user = new SiteUser {UserName = model.Email, Email = model.Email, Name = model.FirstName, CustomerId = customer.Id};
                var result = await userManager.CreateAsync(user, model.Password);
               // await signInManager.SignInAsync(user, false);

                if (result.Succeeded)
                {
                    await signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    anonHandler.RemoveAnonymousId(); 
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return View("Index", model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to register.Please make sure that a user name exists, ");
                ModelState.AddModelError(string.Empty, "the user email has not been used previously.");
                //ModelState.AddModelError(string.Empty, "and the password follows these rules: ");
                //ModelState.AddModelError(string.Empty, "Password is 8 characters in length");
                //ModelState.AddModelError(string.Empty, "Password is 8 characters in length");
                //ModelState.AddModelError(string.Empty, "At least one digit is included");
                //ModelState.AddModelError(string.Empty, "At least one lowercase letter");
                //ModelState.AddModelError(string.Empty, "At least one uppercase letter");  
                return View("Index", model);
            }


            return RedirectToActionPermanent("Index", "Home");
        }
    }
}