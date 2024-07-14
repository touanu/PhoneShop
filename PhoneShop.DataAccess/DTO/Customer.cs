using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Models
{
    public class Customer
    {
        [Key] public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public DateTime? TokenExpiredTime { get; set; }
        public DateTime? RefreshTokenExpired { get; set; }
    }
}
