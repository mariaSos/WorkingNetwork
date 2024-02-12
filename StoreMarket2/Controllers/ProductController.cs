using Microsoft.AspNetCore.Mvc;
using StoreMarket.Abstractions;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;
using System.Text;

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

            try
            {
                var id = _productServices.AddProduct(request);
                return Ok(id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete]
        [Route("productsDelete/{id}")]

        public ActionResult<ProductResponse> RemoveProduct(ProductDeleteRequest request)
        {
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

        //Доработайте контроллер, реализовав в нем метод возврата CSV-файла с товарами.

        private string GetCsv(IEnumerable<ProductResponse> products)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var p in products)
            {
                stringBuilder.Append(p.Name + ";" + p.Price + "\n");
            }

            return stringBuilder.ToString();
        }


        [HttpGet(template:"GetProductsCSV")]
        public FileContentResult GetProductsCsv()
        {
            var products = _productServices.GetProducts();
            var content = GetCsv(products);

            return File(new System.Text.UTF8Encoding().GetBytes(content),"text/csv","report.csv");
        }
        /*-------------------------------------------------------------------------------------------*/
        
    }
}
