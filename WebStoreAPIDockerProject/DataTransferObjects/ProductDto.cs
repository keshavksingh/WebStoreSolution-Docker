namespace StoreWebAPIApplication.DataTransferObjects
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int? ProductPrice { get; set; }
        public DateTime DateModified { get; set; }
    }
}
