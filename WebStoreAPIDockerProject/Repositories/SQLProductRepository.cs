using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreWebAPIApplication.Data;
using StoreWebAPIApplication.DomainModels;

namespace StoreWebAPIApplication.Repositories
{
    public class SQLProductRepository : IProductRepository
    {
        private readonly ProductStoreDbContext dbProductContext;

        public SQLProductRepository(ProductStoreDbContext dbProductContext)
        {
            this.dbProductContext = dbProductContext;
        }
        public async Task<List<Product>> GetAllAsync([FromQuery] string? sortBy, [FromQuery] bool isAscending,int pageNumber=1,int pageSize=1000)
        {
            var products = dbProductContext.Product.AsQueryable();
            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("ProductName", StringComparison.OrdinalIgnoreCase)) 
                {
                    products = isAscending ? products.OrderBy(x=>x.ProductName):products.OrderByDescending(x=>x.ProductName);
                }

            }
            //pagination
            var skipResults = (pageNumber - 1) * pageSize;


            return await products.Skip(skipResults).Take(pageSize).ToListAsync();

        }

        public async Task<Product?> GetByProductIdAsync(Guid ProductId)
        {
            return await dbProductContext.Product.FindAsync(ProductId);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await dbProductContext.Product.AddAsync(product);
            await dbProductContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(Guid ProductId, Product product)
        {
            var existingProduct = await dbProductContext.Product.FirstOrDefaultAsync(x => x.ProductId == ProductId);
            if (existingProduct == null)
            {
                return null;
            }
            else
            {
                existingProduct.ProductPrice = product.ProductPrice;
                await dbProductContext.SaveChangesAsync();
                return existingProduct;
            }
        }

        async Task<Product?> IProductRepository.DeleteAsync(Guid ProductId)
        {
            var existingProduct = await dbProductContext.Product.FirstOrDefaultAsync(x => x.ProductId == ProductId);
            if (existingProduct == null)
            {
                return null;
            }
            else
            {
                dbProductContext.Product.Remove(existingProduct);
                await dbProductContext.SaveChangesAsync();
                return existingProduct;
            }
        }
        public async Task<List<Store>> GetAllProductStoreAsync(string? filterOn = null, string? filterQuery = null)
        {
            var stores = dbProductContext.Store.AsQueryable();
            //Filter
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("StoreName", StringComparison.OrdinalIgnoreCase))
                {

                    stores = stores.Where(x => x.StoreName.Contains(filterQuery));
                }
            }
            return await stores.ToListAsync();
        }
    }
}
