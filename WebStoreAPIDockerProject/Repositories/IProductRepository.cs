using Microsoft.EntityFrameworkCore.Query.Internal;
using StoreWebAPIApplication.DomainModels;

namespace StoreWebAPIApplication.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>>GetAllAsync(string?sortBy=null,bool isAscending=true,int pageNumber=1,int pageSize=1000);
        Task<List<Store>> GetAllProductStoreAsync(string?filterOn=null,string?filterQuery=null);
        Task<Product?> GetByProductIdAsync(Guid ProductId);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(Guid ProductId, Product product);
        Task<Product?> DeleteAsync(Guid ProductId);
    }
}
