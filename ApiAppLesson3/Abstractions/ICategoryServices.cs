using ApiAppLesson3.Contracts.Requests.Categories;
using ApiAppLesson3.Contracts.Responses;

namespace ApiAppLesson3.Abstractions
{
    public interface ICategoryServices
    {
        public int AddCategory(CategoryCreateRequest category);
        
        public IEnumerable<CategoryResponse> GetCategories();
        
        public CategoryResponse GetCategoryById(int id);

    }
}
