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
        public IBrandServices _BrandServices { get; set; }
        public IAccountServices _accountServices { get; set; }
        public IProductServices _productServices { set; get; }
        public IPromotionServices _promotionsServices { set; get; }
        public ICategoryServices _categoryServices { set; get; }

        public INewServices _NewServices { get; set; }

        int SaveChange();
        void Dispose();
    }
}
