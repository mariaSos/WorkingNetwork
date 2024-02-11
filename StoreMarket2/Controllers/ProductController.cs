using Microsoft.AspNetCore.Mvc;
using StoreMarket.Abstractions;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class ProductController : ControllerBase
    {

        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet]
        [Route("products/{id}")]

        public ActionResult<ProductResponse> GetProductById(int id)
        {

            var product = _productServices.GetProductById(id);

            return Ok(product);

        }


        [HttpGet]
        [Route("products")]

        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            var products = _productServices.GetProducts();

            return Ok(products);
        }

        [HttpPost]
        [Route("addProduct")]

        public ActionResult<ProductResponse> AddProducts(ProductCreateRequest request)
        {
            
            try {
                var id = _productServices.AddProduct(request);
                return Ok(id);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [HttpDelete]
        [Route("productsDelete/{id}")]

        public ActionResult<ProductResponse> RemoveProduct(ProductDeleteRequest request) { 
        try
        {
            var id = _productServices.RemoveProduct(request);
            return Ok(id);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        
    }
}
