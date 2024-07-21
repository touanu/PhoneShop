﻿using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IAttributeServices _Attributesservices { get; }
        public IBrandServices _BrandServices { get; }
        public IAccountServices _accountServices { get; set; }
        public IProductServices _productServices { set; get; }
        public IPromotionServices _promotionsServices { set; get; }
        public ICategoryServices _categoryServices { set; get; }
        int SaveChange();
    }
}
