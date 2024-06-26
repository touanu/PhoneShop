﻿using Microsoft.EntityFrameworkCore;
using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PhoneShop.DataAccess.Services
{
    public class AttributesServices : IAttributesservices
    {
        private PhonShopDBcontext dbcontext;
        public async Task<ReturnData> AddAttributes(AttributesRequestData requestData)
        {
            AttributesReturnData result = new AttributesReturnData();
            var errItem = string.Empty;
            try
            {

                if (requestData != null
                    && requestData.AttributesNameValue != null
                    && requestData.AttributeValuestring != null
                    && requestData.ProductID >0)
                    {
                    //them nhom thuoc tinh
                    var GroupAtri_cout = requestData.AttributesNameValue.Split('_').Length;
                    for (int i = 0; i < GroupAtri_cout; i++)
                    {
                        var Grattr_name = requestData.AttributesNameValue.Split('_')[i];
                        // kiểm tra xem null 

                        if (string.IsNullOrEmpty(Grattr_name))
                        {
                            errItem += "tên thuộc tính bị trống hoặc không hợp lệ ";
                            continue;
                        }


                        var GroupAttr_Req = new ProductAttributes()
                        {
                            AttributesName = Grattr_name,
                            ProductID = requestData.ProductID,
                        };

                        dbcontext.productAttributes.Add(GroupAttr_Req);

                    }
                    //them thuoc tinh
                    var attr_count = requestData.AttributeValuestring.Split('_').Length;

                    for (int i = 0; i < attr_count; i++)
                    {
                        var item = requestData.AttributeValuestring.Split('_')[i];

                        var attr_name = item.Split(',')[0];
                        var attr_Price = item.Split(',')[2];

                        var attr_priceSale = item.Split(',')[3];
                        var attr_Quantity = item.Split(',')[1];

                        // kiểm tra xem null 

                        if (string.IsNullOrEmpty(attr_name))
                        {
                            errItem += "tên thuộc tính bị trống hoặc không hợp lệ ";
                            continue;
                        }

                        if (string.IsNullOrEmpty(attr_Quantity))
                        {
                            errItem += "thuộc tính số lượng bị trống";
                            continue;
                        }

                        if (string.IsNullOrEmpty(attr_Price))
                        {
                            errItem += " thuộc tính giá bị trống";
                            continue;
                        }

                        if (string.IsNullOrEmpty(attr_priceSale))
                        {
                            errItem += " thuộc tính giá sale bị trống";
                            continue;
                        }

                        var attr_Req = new ProductAttributesVallues()
                        {
                            AttributeValluesName = attr_name,
                            Quantity = Convert.ToInt32(attr_Quantity),
                            Price = Convert.ToInt32(attr_Price),
                            PriceSale = Convert.ToInt32(attr_priceSale),
                        };

                        dbcontext.attributesVallues.Add(attr_Req);
                    }
                   dbcontext.SaveChangesAsync();

                    result.ReturnCode = 1;
                    result.ReturnMsg = "Thêm sản phẩm thành công";
                    return result;
                }
                result.ReturnCode = -1;
                result.ReturnMsg = "Dữ liệu vào không hợp lệ!";
                return result;


            }
            catch (Exception ex)
            {
                result.ReturnCode = -1;
                result.ReturnMsg = "Hệ thống đang bận:" + ex.Message;
                return result;
            }
        }

        public async Task<ReturnData> DeleteAtributesVallue(AttributesRequestData requestData)
        {
            try
            {
                ReturnData returnData = new ReturnData();
                if(requestData.AttributeValluesName!=null)
                {
                    returnData.ReturnCode = -1;
                    returnData.ReturnMsg = "dữ liệu vào không hợp lệ!";
                    return returnData;
                }
                var AttributesValue = new ProductAttributesVallues();
                foreach (var attr in dbcontext.attributesVallues)
                {
                    if (attr.AttributeValluesName == requestData.AttributeValluesName)
                    {
                        AttributesValue= attr;
                    }
                }
                dbcontext.attributesVallues.Remove(AttributesValue);
                dbcontext.SaveChanges();
                returnData.ReturnCode = -1;
                returnData.ReturnMsg = "xóa thành công biến thể có tên"+requestData.AttributeValluesName;
                return returnData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<ReturnData> DeleteAttributes(AttributesRequestData requestData)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnData> UpdateAttributes(AttributesRequestData requestData)
        {
            throw new NotImplementedException();
        }
    }
}
