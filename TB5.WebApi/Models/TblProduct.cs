using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TB5.WebApi.Models
{
    [Table("TblProduct")]
    public class TblProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool IsDelete { get; set; }
    }

    public class ProductCreateRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }

    public class ProductCreateResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}
