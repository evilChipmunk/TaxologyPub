 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taxology.Site.Models;

namespace Taxology.Site.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }
         

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {  
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
          
        public IActionResult Error(ErrorViewModel model)
        {
            return View(model);
        }
    }
}
