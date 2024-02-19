using ApiAppLesson3.Abstractions;
using ApiAppLesson3.Contexts;
using ApiAppLesson3.Contracts.Requests;
using ApiAppLesson3.Contracts.Responses;
using ApiAppLesson3.Models;
using AutoMapper;

namespace ApiAppLesson3.Services
{
    public class StoreService : IStoreServices
    {
        private readonly IMapper _mapper;
        private readonly StoreContext _storeContext;

        public StoreService(StoreContext storeContext, IMapper mapper)
        {
            _storeContext = storeContext;
            _mapper = mapper;
        }
        public int AddStore(StoreCreateRequest createStoreRequest)
        {
            var mapEntity = _mapper.Map<Store>(createStoreRequest);
            _storeContext.Stores.Add(mapEntity);
            _storeContext.SaveChanges();

            return mapEntity.Id;
        }

        public IEnumerable<ProductResponse> GetProductsInStore(int id)
        {
            Store? store = _storeContext.Stores.FirstOrDefault(s => s.Id == id);
            IEnumerable<ProductResponse> products = store.Products.Select(_mapper.Map<ProductResponse>).ToList();

            return products;
        }
        public int GetCountInStore(int id)
        {
            Store? store = _storeContext.Stores.FirstOrDefault(s => s.Id == id);
            return store.Count;
        }
    }
}

