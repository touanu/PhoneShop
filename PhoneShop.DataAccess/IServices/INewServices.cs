using PhoneShop.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.IServices
{
    public interface INewServices
    {
        Task<List<News>> NewGetList();
        Task<List<News>> NewGetList(NewRequestDaTa requestDaTa);
        Task<NewInsertUpdateReturnData> NewInsertUpdate (NewInsertUpdateRequestDaTa requestDaTa);
        Task<NewDeleteReturnData> NewDelete (NewDeleteRequestDaTa requestDaTa);

    }
}
