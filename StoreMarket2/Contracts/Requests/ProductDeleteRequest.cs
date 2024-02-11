using StoreMarket.Models;

namespace StoreMarket.Contracts.Requests
{
    public class ProductDeleteRequest
    {
        public int Id { get; set; }

        public Product ProductGetEntity()
        {

            return new Product()
            {
                Id = Id,
                Name = string.Empty
            };
        }
    }
}
