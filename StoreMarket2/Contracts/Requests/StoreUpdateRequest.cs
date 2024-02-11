using StoreMarket.Models;

namespace StoreMarket.Contracts.Requests
{
    public class StoreUpdateRequest
    {
        public int Id { get; set; }
        public int Count { get; set; } = 0;


        public Store StoreGetEntity()
        {

            return new Store()
            {
                Id = Id,
                Count = Count
            };
        }
    }
}

