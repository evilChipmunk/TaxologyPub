using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;

namespace Taxology.Site.Controllers
{
    public class BaseWebController : Controller
    {
        private readonly UserManager<SiteUser> userManager; 
 

        public BaseWebController(UserManager<SiteUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<SiteUser> GetSiteUser()
        {
            return await userManager.GetUserAsync(User);
        }

    }
}