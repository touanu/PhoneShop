using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IAttributesservices _Attributesservices { get; }
        //public IBrandServices _BrandServices { get; }
        public IAccountServices _accountServices { get; set; }
        int SaveChange();
    }
}
