using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.API.Common;
using SmartFarmManager.Service.BusinessModels.DeadPoultryLog;
using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.Interfaces;
using System;

namespace SmartFarmManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeadPoultryLogController : ControllerBase
    {
        private readonly IDeadPoultryLogService _deadPoultryLogService;

        public DeadPoultryLogController(IDeadPoultryLogService deadPoultryLogService)
        {
            _deadPoultryLogService = deadPoultryLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDeadPoultryLogs([FromQuery] string? cageName, [FromQuery] string? farmingBatchName, [FromQuery] string? note,[FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _deadPoultryLogService.GetDeadPoultryLogsAsync(cageName, farmingBatchName, note, startDate, endDate, pageNumber, pageSize);

            return Ok(ApiResult<PagedResult<DeadPoultryLogResponseModel>>.Succeed(result));
        }
    }
}
