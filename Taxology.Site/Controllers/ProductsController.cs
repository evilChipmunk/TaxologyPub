 
using System.Collections.Generic; 
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.DTO;
using ShoppingCart.DTO;
using Taxology.Site.Models;
using Taxology.Site.Services;

namespace Taxology.Site.Controllers
{ 
    public class ProductsController : Controller
    { 
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IShoppingCartService shoppingCartService;

        public ProductsController(IProductService productService, IMapper mapper, IShoppingCartService shoppingCartService)
        { 
            this.productService = productService;
            this.mapper = mapper;
            this.shoppingCartService = shoppingCartService;
        }
         

        public async Task<IActionResult> Index()
        { 
            var productListModel = await GetProductListModel();
             
            return View(productListModel);
        }

        private async Task<ProductListModel> GetProductListModel()
        {
            if (ViewBag.productListModel != null) return ViewBag.productListModel as ProductListModel;

            var products = await productService.GetProducts();
            var productListModel = new ProductListModel();
            productListModel.Products = mapper.Map<IEnumerable<ProductModel>>(products);
            ViewBag.productListModel = productListModel;
            return productListModel;
        }


        public async Task<IActionResult> AddToCart(ProductModel product)
        {
            var cartModel = await shoppingCartService.GetShoppingCart();
            var shoppingCartProductDTO = mapper.Map<ShoppingCartProductDTO>(product);

            var success = await shoppingCartService.AddProduct(cartModel, shoppingCartProductDTO);
 
            if (!success)
            {
                ModelState.AddModelError("summaryError", "Unable to add product, please try again");
            }   
             
            return RedirectToAction("AddedToCart", product);
        }


        public async Task<IActionResult> AddedToCart(ProductModel product)
        {

            return RedirectToAction("Index", "ShoppingCart");

            var productListModel = await GetProductListModel();
            var suggestedProducts = productListModel.Products;

            ProductAddedConfirmationModel confirmation = new ProductAddedConfirmationModel(product, suggestedProducts);
            return View(confirmation);
        }

        public IActionResult GoToCart()
        {
            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult Details(ProductModel product)
        {
 

            var productDTO = mapper.Map<ProductDTO>(product);
            var link = new ProductCatalogLink(this.Url).GetLink(productDTO);
            return Redirect(link);


            if (product.Name == "Tax Filing")
            {
                return View("Filing", product);
            }
            else if (product.Name == "Accounting Advice")
            {
                return View("Advice", product);
            }
            else if (product.Name == "Audit Defense")
            {
                return View("Audit", product);
            }
            else if (product.Name == "Appraisal")
            {
                return View("Appraisal", product);
            }

            return BadRequest();
        }


        public IActionResult Filing()
        {
            return View();
        }

        public IActionResult Advice()
        {
            return View();
        }

        public IActionResult Audit()
        {
            return View();
        }

        public IActionResult Appraisal()
        {
            return View();
        }
    }
}
