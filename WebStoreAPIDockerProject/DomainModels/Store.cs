namespace StoreWebAPIApplication.DomainModels
{
    public class Store
    {
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public Guid ProductId { get; set; }

        public Product product { get; set; }
    }
}
