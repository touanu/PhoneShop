using PhoneShop.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.IServices
{
    public interface ICategoryServices
    {
        Task<List<Category>> GetAllCategories();
        Task<GetCategoryReturnData> GetCategories(CategoryRequestData requestData);
        Task<ReturnData> AddCategory(CategoryRequestData RequestData);
        Task<ReturnData> UpdateCategory(CategoryRequestData RequestDataData);
        Task<ReturnData> DeleteCategory(CategoryRequestData requestData);
    }
}
