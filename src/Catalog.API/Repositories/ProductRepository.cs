using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;
        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }
        public async  Task CreateProduct(Product product)
        {
             await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id,id);
            DeleteResult deleteReuslt = await _catalogContext.Products.DeleteOneAsync(filter);
            return deleteReuslt.IsAcknowledged && deleteReuslt.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _catalogContext.Products.Find(p=>p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            return await _catalogContext.Products.Find(p => p.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _catalogContext.Products.Find(p => p.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext.Products.Find(p => true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
          var updateResult=   await _catalogContext.Products.ReplaceOneAsync(filter:g=>g.Id==product.Id, replacement:product);
           return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
            
        }
    }
}
