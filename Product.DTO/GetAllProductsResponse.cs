using System.Collections.Generic;
using Shared.DTO;

namespace Product.DTO
{
    public class GetAllProductsResponse : BaseResponse
    {
        public GetAllProductsResponse()
        {

        }
        public GetAllProductsResponse(IEnumerable<ProductDTO> products) : base(true, ResponseAction.Found)
        {
            Products = products;
        }

        public IEnumerable<ProductDTO> Products { get; set; }
    }
}