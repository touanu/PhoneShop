using PhoneShop.DataAccess.DTO;
using PhoneShop.DataAccess.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.Services
{
   public class NewServices : INewServices 
    {
        PhoneShopDBcontext dbcontext;

        public NewServices(PhoneShopDBcontext _dbcontext)
        {
            dbcontext = _dbcontext;
        }

      

        public Task<NewDeleteReturnData> NewDelete(NewDeleteRequestDaTa requestDaTa)
        {
            throw new NotImplementedException();
        }

        public async Task<List<News>> NewGetList()
        {
            var returnDataNew = new List<News>();
            try
            {
                returnDataNew = dbcontext.News.ToList();

                return (returnDataNew);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Task<List<News>> NewGetList(NewRequestDaTa requestDaTa)
        {
            throw new NotImplementedException();
        }

        public async Task<NewInsertUpdateReturnData> NewInsertUpdate(NewInsertUpdateRequestDaTa requestDaTa)
        {
            var returndata = new NewInsertUpdateReturnData();
            var errItem = string.Empty;
            try
            {
                if (requestDaTa == null || string.IsNullOrEmpty(requestDaTa.NewsName))
                {
                    returndata.ReturnCode = -1;
                    returndata.ReturnMsg = "du lieu khong hop le";
                    return returndata;
                }
               var News = dbcontext.News.Where(s => s.NewsName == requestDaTa.NewsName).FirstOrDefault();

            if(News == null || News.newsID>0)
                {  returndata.ReturnCode = -2;
                    returndata.ReturnMsg = "thong tin da ton tai";
                    return returndata;
                }


                var NewsReq = new News
                {
                    NewsName = requestDaTa.NewsName,
                    NewsMain = requestDaTa.NewsMain,
                };
           
                dbcontext.News.Add(NewsReq);

                dbcontext.SaveChanges();
                return returndata;
           
            }
                  
            catch (Exception)
            {

                throw;
            }
        }
    }
}
