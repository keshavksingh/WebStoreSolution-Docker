using StoreWebAPIApplication.DomainModels;

namespace StoreWebAPIApplication.DataTransferObjects
{
    public class StoreDto
    {
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public Guid ProductId { get; set; }

    }
}
