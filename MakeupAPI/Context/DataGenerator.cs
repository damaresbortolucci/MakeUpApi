using MakeupAPI.Model;
using System.Text.Json;

namespace MakeupAPI.Context
{   
    public class DataGenerator
    {
        private readonly InMemoryContext _inMemoryContext;

        public DataGenerator(InMemoryContext inMemoryContext)
        {
            _inMemoryContext = inMemoryContext;
        }

        public void Generate()
        {
            if (!_inMemoryContext.Product.Any())
            {
                List<Product> items;
                using (StreamReader r = new StreamReader("data.json"))
                {
                    string json = r.ReadToEnd();
                    items = JsonSerializer.Deserialize<List<Product>>(json);
                }

                _inMemoryContext.Product.AddRange(items);
                _inMemoryContext.SaveChanges();
            }
        }
    }
}

