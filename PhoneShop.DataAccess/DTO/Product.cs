using System.ComponentModel.DataAnnotations;

namespace PhoneShop.DataAccess.DTO
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public int? BrandID { get; set; }
        public int? CategoryID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? Images { get; set; }
    }
    public class ResponseProduct : Product
    {
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
    }
}
