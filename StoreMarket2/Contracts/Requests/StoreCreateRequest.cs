using StoreMarket.Models;

namespace StoreMarket.Contracts.Requests
{
    public class StoreCreateRequest
    {
        public int Count { get; set; } = 0;


        public Store StoreGetEntity()
        {

            return new Store()
            {
                Count = Count
            };
        }
    }
}
