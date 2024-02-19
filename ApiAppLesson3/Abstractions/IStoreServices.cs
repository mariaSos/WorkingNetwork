using ApiAppLesson3.Contracts.Requests;
using ApiAppLesson3.Contracts.Requests.Products;
using ApiAppLesson3.Contracts.Responses;

namespace ApiAppLesson3.Abstractions
{
    public interface IStoreServices
    {
        int AddStore(StoreCreateRequest CreateStoreRequest);
       
        IEnumerable<ProductResponse> GetProductsInStore(int id);
        
        int GetCountInStore(int id);

    }
}
