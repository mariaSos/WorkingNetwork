using StoreMarket.Models;

namespace StoreMarket.Contracts.Requests
{
    public class StoreDeleteRequest
    {
        public int Id { get; set; }


        public Store StoreGetEntity()
        {

            return new Store()
            {
                Id = Id
            };
        }
    }
}
