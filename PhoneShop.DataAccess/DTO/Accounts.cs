using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Models
{
    public class Accounts
    {
        [Key]public int AccountID { get; set; }
        [Required(ErrorMessage = "Tên không được để trống.")]
        public string FristName { get; set; }
        [Required(ErrorMessage = "Tên không được để trống.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt.")]
        public string UserName {get;set;}
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string PassWord { get; set; }
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email không được để trống.")]
        public string Email { get; set; }
        public int ProviceID { get; set; }
        public int DistrictID { get; set; }
        public int WardsID { get; set; }
    }
}
