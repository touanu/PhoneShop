using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;
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

        public INewServices _NewServices { get; set; }

        public IBrandServices _BrandServices {  set; get; }

        public IAccountServices _accountServices {  set; get; }

        public IPromotionServices _promotionsServices { set; get; }

        public IProductServices _productServices { set; get; }

        public ICategoryServices _categoryServices { set; get; }
        public UnitOfWork(IProductServices productServices,IBrandServices brandServices,PhoneShopDBcontext dBcontext, IAccountServices accountServices,IPromotionServices promotionsServices,ICategoryServices categoryServices ,INewServices newServices)
        {
            _accountServices = accountServices;
            _dbcontext = dBcontext;
            _BrandServices = brandServices;
            _promotionsServices = promotionsServices;
            _categoryServices = categoryServices;
            _productServices = productServices;
            _NewServices = newServices;
        }

        public int SaveChange()
        {
            return _dbcontext.SaveChanges();
        }
        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}
