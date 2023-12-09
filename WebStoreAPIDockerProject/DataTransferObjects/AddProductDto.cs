using System.ComponentModel.DataAnnotations;

namespace StoreWebAPIApplication.DataTransferObjects
{
    public class AddProductDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="Product Name Need to be Minimum 3 Characters.")]
        [MaxLength(100, ErrorMessage = "Product Name Need to be Maximum Of 100 Characters.")]
        public string ProductName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ProductPrice must be a positive integer.")]
        public int ProductPrice { get; set; }
        public DateTime DateModified { get; set; }= DateTime.Now;
    }
}
