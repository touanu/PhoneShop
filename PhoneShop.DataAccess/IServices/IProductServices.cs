using PhoneShop.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.IServices
{
    public interface IProductServices
    {
        Task<List<Product>> GetProducts();
        Task<GetProductReturnData> GetProductById(int id);
        Task<ReturnData> InsertProduct(ProductRequestAddUpdateData requestData);
    }
}
