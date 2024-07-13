using System.ComponentModel.DataAnnotations;

namespace PhoneShop.DataAccess.DTO
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public int BrandID { get; set; }
        public int CategoryID { get; set; }
        public required string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public required string Images { get; set; }
    }
}
