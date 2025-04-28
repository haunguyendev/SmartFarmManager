using SmartFarmManager.Service.BusinessModels.CostingReport;
using SmartFarmManager.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Interfaces
{
    public interface ICostingService
    {
        Task CalculateAndStoreDailyCostAsync();
        Task<PagedResult<CostingReportItemModel>> GetCostingReportsAsync(CostingReportFilterModel filter);
        Task<PagedResult<CostingReportGroupModel>> GetGroupedCostingReportsAsync(CostingReportGroupFilterModel filter);
    }

}
