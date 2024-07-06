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
        private PhoneShopDBcontext _dbcontext { set; get; }
        public IAttributesservices _Attributesservices {  set; get; }

        public IBrandServices _BrandServices {  set; get; }

        public IAccountServices _accountServices {  set; get; }

        public IPromotionsServices _promotionsServices { set; get; }

        public IProductServices _productServices { set; get; }
        public UnitOfWork(IAttributesservices attributesservices,IBrandServices brandServices,PhoneShopDBcontext dBcontext, IAccountServices accountServices,IPromotionsServices promotionsServices)
        {
            _accountServices = accountServices;
            _dbcontext = dBcontext;
            _BrandServices = brandServices;
            _Attributesservices = attributesservices;
            _promotionsServices = promotionsServices;
        }

        public int SaveChange()
        {
            return _dbcontext.SaveChanges();
        }
    }
}
