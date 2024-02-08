using Microsoft.AspNetCore.Mvc;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("categories/{id}")]

        public ActionResult<CategoryResponse> GetCategory(int id)
        {
            var result = storeContext.Categories.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new CategoryResponse(result));
            }

        }


        [HttpGet]
        [Route("categories")]

        public ActionResult<IEnumerable<CategoryResponse>> GetCategories()
        {
            var result = storeContext.Categories;

            return Ok(result.Select(result => new CategoryResponse(result)));
        }

        [HttpPost]
        [Route("categories")]

        public ActionResult<CategoryResponse> AddCategories(CategoryCreateRequest request)
        {
            Category category = request.CategoryGetEntity();
            try
            {
                var result = storeContext.Categories.Add(category).Entity;

                storeContext.SaveChanges();
                return Ok(new CategoryResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete]
        [Route("categoriesDelete/{id}")]

        public ActionResult<CategoryResponse> DeleteCategories(CategoryDeleteRequest request)
        {
            Category category = request.CategoryGetEntity();
            try
            {
                var result = storeContext.Categories.Remove(category).Entity;

                storeContext.SaveChanges();
                return Ok(new CategoryResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        private StoreContext storeContext;

        public CategoryController(StoreContext context)
        {
            storeContext = context;

        }
    }
}
