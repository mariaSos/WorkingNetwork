using StoreMarket.Models;

namespace StoreMarket.Contracts.Responses
{
    public class StoreResponse
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public StoreResponse(Store store)
        {
            Id = store.Id;
            Count = store.Count;

        }
    }
}
