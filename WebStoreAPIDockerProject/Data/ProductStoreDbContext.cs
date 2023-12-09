using Microsoft.EntityFrameworkCore;
using StoreWebAPIApplication.DomainModels;

namespace StoreWebAPIApplication.Data
{
    public class ProductStoreDbContext: DbContext
    {
        public ProductStoreDbContext(DbContextOptions dbContextOptions):base (dbContextOptions)
        {
                
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Store> Store { get; set; }    
    }
}
