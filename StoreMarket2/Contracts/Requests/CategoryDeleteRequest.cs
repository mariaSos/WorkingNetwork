using StoreMarket.Models;

namespace StoreMarket.Contracts.Requests
{
    public class CategoryDeleteRequest
    {
        public int Id { get; set; }


        public Category CategoryGetEntity() => new Category()
        {

            Id = Id,
            Name = string.Empty
        };
      
    }
}
