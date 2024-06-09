using System.ComponentModel.DataAnnotations;

namespace PhoneShop.DataAccess.DTO
{
    public class LoginRequestData
    {
        [Key] public int ID { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ProviceID { get; set; }
        public int DistrictID { get; set; }
        public int WardsID { get; set; }
    }
}
