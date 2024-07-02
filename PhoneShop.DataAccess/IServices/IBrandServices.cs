using PhoneShop.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.IServices
{
    public interface IBrandServices
    {
        Task<List<Brands>> BrandsGetList();
        Task<BrandInsertReturnData> BrandInsertUpdate(BrandInsertRequestData requestData);

        Task<Brand_DeleteReturnData> Brand_Delete(Brand_DeleteRequestData requestData);
       
    }

    
}
