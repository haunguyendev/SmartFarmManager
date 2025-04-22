using SmartFarmManager.Service.BusinessModels.DeadPoultryLog;
using SmartFarmManager.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Interfaces
{
    public interface IDeadPoultryLogService
    {
        Task<PagedResult<DeadPoultryLogResponseModel>> GetDeadPoultryLogsAsync(bool? isError,string? cageName, string? farmingBatchName, string? note, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
        Task<bool> ReportErrorAndResetQuantityAsync(Guid deadPoultryLogId, string reportNote);
    }
}
