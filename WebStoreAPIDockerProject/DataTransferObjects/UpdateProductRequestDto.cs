using System.ComponentModel.DataAnnotations;

namespace StoreWebAPIApplication.DataTransferObjects
{
    public class UpdateProductRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ProductPrice must be a positive integer.")]
        public int ProductPrice { get; set; }
    }
}
