using StoreMarket.Models;

namespace StoreMarket.Contracts.Requests
{
    public class CategoryUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public Category CategoryGetEntity()
        {

            return new Category()
            {
                Id = Id,
                Name = Name,
                Description = Description
            };
        }
    }
}
