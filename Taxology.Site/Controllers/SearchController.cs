using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taxology.Site.Services;

namespace Taxology.Site.Controllers
{
    public class SearchController : Controller
    {
        private readonly IProductService productService;

        public SearchController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index(string term)
        {
            var products = await productService.SearchForProducts(term);
             
            List<SearchResultModel> results = new List<SearchResultModel>();
            foreach (var product in products)
            {
                results.Add(new SearchResultModel
                {
                    Item = product.Name,
                    Description = product.Description,
                    Link = new ProductCatalogLink(this.Url).GetLink(product)

                });
            }
             
            var model = new SearchModel();
            model.Results = results;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string term)
        {
            return RedirectToAction("Index", "Search", new {term = term});// View("Index", model);
        }
    }

    public class SearchModel
    {
        public SearchModel()
        {
            Results = new List<SearchResultModel>();
        }
        public int ResultCount
        {
            get { return Results.Count(); }
        }
        public IEnumerable<SearchResultModel> Results { get; set; }
    }

    public class SearchResultModel
    {
        public string Item { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}