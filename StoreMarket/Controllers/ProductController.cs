using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        [Route("products/{id}")]

        public ActionResult<ProductResponse> GetProduct(int id)
        {
            var result = storeContext.Products.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new ProductResponse(result));
            }

        }


        [HttpGet]
        [Route("products")]

        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            var result = storeContext.Products;

            return Ok(result.Select(result => new ProductResponse(result)));
        }

        [HttpPost]
        [Route("products")]

        public ActionResult<ProductResponse> AddProducts(ProductCreateRequest request)
        {
            Product product = request.ProductGetEntity();
            try {
                var result = storeContext.Products.Add(product).Entity;

                storeContext.SaveChanges();
                return Ok(new ProductResponse(result));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [HttpDelete]
        [Route("productsDelete/{id}")]

        public ActionResult<ProductResponse> DeleteProduct(ProductDeleteRequest request)
        {
            Product product = request.ProductGetEntity();
            try
            {
                var result = storeContext.Products.Remove(product).Entity;

                storeContext.SaveChanges();
                return Ok(new ProductResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        private StoreContext storeContext;

        public ProductController(StoreContext context)
        {
            storeContext = context;

        }
    }
}
