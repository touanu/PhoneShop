using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private PhonShopDBcontext _dbcontext { set; get; }
        public IAttributesservices _Attributesservices {  set; get; }

        //public IBrandServices _BrandServices {  set; get; }

        public IAccountServices _accountServices {  set; get; }
        public UnitOfWork(IAttributesservices attributesservices,/*IBrandServices brandServices*/PhonShopDBcontext dBcontext, IAccountServices accountServices)
        {
            _accountServices = accountServices;
            _dbcontext = dBcontext;
            //_BrandServices = brandServices;
            _Attributesservices = attributesservices;
        }

        public int SaveChange()
        {
            return _dbcontext.SaveChanges();
        }
    }
}
