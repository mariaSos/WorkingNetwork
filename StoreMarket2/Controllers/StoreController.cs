using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        [HttpGet]
        [Route("stores/{id}")]

        public ActionResult<StoreResponse> GetStore(int id)
        {
            var result = storeContext.Stores.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new StoreResponse(result));
            }

        }


        [HttpGet]
        [Route("stores")]

        public ActionResult<IEnumerable<StoreResponse>> GetStores()
        {
            var result = storeContext.Stores;

            return Ok(result.Select(result => new StoreResponse(result)));
        }

        [HttpPost]
        [Route("stores")]

        public ActionResult<ProductResponse> AddStores(StoreCreateRequest request)
        {
            Store store = request.StoreGetEntity();
            try
            {
                var result = storeContext.Stores.Add(store).Entity;

                storeContext.SaveChanges();
                return Ok(new StoreResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete]
        [Route("storesDelete/{id}")]

        public ActionResult<StoreResponse> DeleteStore(StoreDeleteRequest request)
        {
            Store store = request.StoreGetEntity();
            try
            {
                var result = storeContext.Stores.Remove(store).Entity;

                storeContext.SaveChanges();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        private StoreContext storeContext;

        public StoreController(StoreContext context)
        {
            storeContext = context;

        }
    }
}
