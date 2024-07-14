using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class UserPermission
    {
        [Key]
        public int UserFunctionID { get; set; }
        public int AccountID { get; set; }
        public int FunctionID { get; set; }
        public int IViews { get; set; }
        public int IUpdate { get; set; }
        public int IsDelete { get; set; }
        public int IsExport { get; set; }
    }
}
