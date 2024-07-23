﻿using PhoneShop.DataAccess.DTO;
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
        public async Task<List<Category>> GetCategories()
        {
            var list = new List<Category>();
            try
            {
                list =_dbcontext.Category.ToList();
                return list;
            }
            catch (Exception ex)
            {

                throw;
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
                    returnData.ReturnMsg = "Tên sản phẩm đã tồn tại";
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
                returnData.ReturnMsg = "Thêm sản phẩm thành công";
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.ReturnCode = -969;
                returnData.ReturnMsg = "Hệ thống đang bận!";
                return returnData;
            }
        }

        public Task<ReturnData> DeleteCategory(string CategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnData> UpdateCategory(CategoryRequestData RequestDataData)
        {
            throw new NotImplementedException();
        }
    }
}