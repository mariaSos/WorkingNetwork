using ApiAppLesson3.Contracts.Requests.Products;
using ApiAppLesson3.Contracts.Responses;

namespace ApiAppLesson3.Abstractions
{
    public interface IProductServices
    {
            public int AddProduct(ProductCreateRequest product);
            public IEnumerable<ProductResponse> GetProducts();

            public ProductResponse GetProductById(int id);
            public bool DeleteProduct(int id);
            public bool UpdatePrice(int idProduct, int price);
            public bool DeleteCategory(string category);

    }
}
