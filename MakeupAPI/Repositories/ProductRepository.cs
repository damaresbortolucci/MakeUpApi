using MakeupAPI.Context;
using MakeupAPI.Interfaces;
using MakeupAPI.Model;

namespace MakeupAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InMemoryContext _context;

        public ProductRepository(InMemoryContext inMemoryContext)
        {
            _context = inMemoryContext;
        }

        

        public Task<IQueryable<Product>> Get(int page, int maxResults)
        {
            return Task.Run(() =>
            {
                var data = _context.Set<Product>().AsQueryable().Skip((page-1) * maxResults).Take(maxResults);
                return data.Any() ? data : new List<Product>().AsQueryable();
            });
        }

        public Task<Product?> GetByKey(int key)
        {
            return Task.Run(() =>
            {
                return _context.Find<Product>(key);
            });
        }


        public Task<IOrderedQueryable<Product>> GetByBrandName(string brand)
        {
            return Task.Run(() =>
            {
                var products = _context.Product.Where(c => c.Brand == brand)
                                                .OrderBy(c => c.Id);
                return products;
            });
        }


        public Task<IQueryable<Product>> GetAverageByType(string type, float minRating)
        {
            return Task.Run(() =>
            {
                var products = _context.Product.Where(x => x.Product_Type== type && x.Rating >= minRating);
                return products;
            });
        }


        public Task<IQueryable<Product>> GetAllBrands()
        {
            return Task.Run(() =>
            {
                var products = _context.Product.Where(x => x.Brand != null);         
                return products;
            });
        }
     

        public Task<Product> Insert(Product entity)
        {
            return Task.Run(() =>
            {
                _context.Add(entity);
                _context.SaveChanges();
                return entity;
            });
        }

        public Task<Product> Update(Product entity)
        {
            return Task.Run(() =>
            {
                _context.Update(entity);
                _context.SaveChanges();
                return entity;
            });
        }


        public Task<int> Delete(int key)
        {
            return Task.Run(() =>
            {
                var entity = _context.Find<Product>(key);

                if (entity == null)
                    throw new KeyNotFoundException();

                _context.Remove(entity);
                _context.SaveChanges();
                return key;
            });
        }
    }
}
