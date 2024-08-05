using Microsoft.EntityFrameworkCore;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.Services
{
    public class CategoryServices : ICategoryServices
    {
        private PhoneShopDBcontext _dbcontext;
        public CategoryServices (PhoneShopDBcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _dbcontext.Category.ToListAsync();
        }
        public async Task<GetCategoryReturnData> GetCategories(CategoryRequestData requestData)
        {
            var list = new List<Category>();
            var returnData = new GetCategoryReturnData();
            try
            {
                list = _dbcontext.Category.ToList();

                if (!string.IsNullOrEmpty(requestData.CategoryName))
                {
                    list = list.FindAll(s => s.CategoryName.ToLower().Contains(requestData.CategoryName.ToLower())).ToList();
                }
                //list = list.FindAll(s=>s.DisplayStatus==1).ToList();
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "lấy dữ liệu thành công!";
                returnData.list = list;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = "Hệ thống đang bận!"+ex;
                return returnData;
            }
        }
        public async Task<ReturnData> AddCategory(CategoryRequestData RequestData)
        {
            ReturnData returnData = new ReturnData();
            try
            {
                var Category = _dbcontext.Category.Where(s => s.CategoryName == RequestData.CategoryName).FirstOrDefault();
                if (Category != null || Category?.CategoryID > 0)
                {
                    returnData.ReturnCode = -2;
                    returnData.ReturnMsg = "Tên danh mục sản phẩm đã tồn tại";
                    return returnData;
                }
                var Category_Req = new Category
                {
                    CategoryName=RequestData.CategoryName,
                    IconImages=RequestData.IconImages,
                    DisplayStatus=RequestData.DisplayStatus,
                };

                _dbcontext.Category.Add(Category_Req);
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "Thêm danh mục sản phẩm thành công";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = "Hệ thống đang bận!";
                return returnData;
            }
        }

        public async Task<ReturnData> DeleteCategory(CategoryRequestData requestData)
        {
            var returnData = new ReturnData();
            try
            {
                if (requestData == null
                || requestData.CategoryID <= 0)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "dữ liệu vào không hợp lệ";
                    return returnData;
                }
                Category category = new Category();
                foreach (var c in _dbcontext.Category)
                {
                    if (c.CategoryID == requestData.CategoryID)
                    {
                        category = c;
                    }
                }
                if (category == null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "Không có danh muc có ID bạn vừa nhập";
                    return returnData;
                }
                _dbcontext.Category.Remove(category);
                returnData.ReturnCode = 1;
                returnData.ReturnMsg = "xóa danh mục thành công";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = "Hệ thống đang bận!"+ex;
                return returnData;
            }
        }

        public Task<ReturnData> UpdateCategory(CategoryRequestData RequestDataData)
        {
            throw new NotImplementedException();
        }
    }
}
