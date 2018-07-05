using Microsoft.AspNetCore.Mvc;
using Product.DTO;

namespace Taxology.Site.Services
{
    public class ProductCatalogLink
    {
        private readonly IUrlHelper urlHelper; 

        public ProductCatalogLink(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper; 
        }
        public string GetLink(ProductDTO product)
        { 

            ///this is a horrible way of doing this - but its quick and cheap for this project
            if (product.Name.ToLower().Contains("filing"))
            {
                return this.urlHelper.Action("Filing", "Products"); 
            }
            else if (product.Name.ToLower().Contains("advice"))
            {
                return this.urlHelper.Action("Advice", "Products");
                // return View("Advice", product);
            }
            else if (product.Name.ToLower().Contains("audit"))
            {
                return this.urlHelper.Action("Audit", "Products");
                // return View("Audit", product);
            }
            else if (product.Name.ToLower().Contains("appraisal"))
            {
                return this.urlHelper.Action("Appraisal", "Products");
                //  return View("Appraisal", product);
            }

            return "";
        }
    }
}