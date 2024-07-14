﻿using PhoneShop.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.IServices
{
    public interface IPromotionServices
    {
        Task<List<Promotion>> GetPromotions(PromotionsRequestData requestData);
        Task<GetPromotionsReturnData> AddPromotions(PromotionsRequestData requestData);
        Task<ReturnData> DeletePromotions(PromotionsRequestData requestData);
        Task<ReturnData> UpdateTimePromotions(PromotionsRequestData requestData);
    }
}