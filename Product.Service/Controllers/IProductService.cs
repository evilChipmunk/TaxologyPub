using System.Threading.Tasks;
using Product.DTO;

namespace Product.Service.Controllers
{
    public interface IProductService
    {
        Task<GetAllProductsResponse> GetProducts(GetAllProductsRequest request);
    }
}