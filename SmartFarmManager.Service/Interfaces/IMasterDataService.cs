using SmartFarmManager.Service.BusinessModels.MasterData;
using SmartFarmManager.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Interfaces
{
    public interface IMasterDataService
    {
        Task<PagedResult<MasterDataItemModel>> GetMasterDataAsync(MasterDataFilterModel filter);
    }
}
