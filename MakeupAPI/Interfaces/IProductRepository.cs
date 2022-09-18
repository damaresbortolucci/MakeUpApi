using MakeupAPI.Model;

namespace MakeupAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<IQueryable<Product>> Get(int page, int maxResults);

        Task<Product?> GetByKey(int key);

        Task<IOrderedQueryable<Product>> GetByBrandName(string brand);

        Task<IQueryable<Product>> GetTypeAndRating(string type, float minRating);

        Task<IQueryable<Product>> GetAllBrands();

        Task<Product> Insert(Product entity);

        Task<Product> Update(Product entity);

        Task<int> Delete(int key);
        
    }
}
