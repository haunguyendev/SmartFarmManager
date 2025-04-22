using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.API.Common;
using SmartFarmManager.Service.BusinessModels.DeadPoultryLog;
using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.Interfaces;
using System;
using SmartFarmManager.API.Payloads.Requests.DeadPoultryLog;

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
        
        [HttpPut("{id}/report-error")]
        public async Task<IActionResult> ReportError(Guid id, [FromBody] ReportErrorRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResult<Dictionary<string, string[]>>.Error(new Dictionary<string, string[]>
                {
                    { "Errors", errors.ToArray() }
                }));
            }

            try
            {
                var result = await _deadPoultryLogService.ReportErrorAndResetQuantityAsync(id, request.ReportNote);

                if (!result)
                {
                    return BadRequest(ApiResult<string>.Fail("Cập nhật báo cáo thất bại. Vui lòng thử lại."));
                }

                return Ok(ApiResult<string>.Succeed("Đã báo cáo nhầm lẫn và cập nhật số lượng về 0 thành công."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResult<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail("Đã xảy ra lỗi không mong muốn. Vui lòng liên hệ bộ phận hỗ trợ."));
            }
        }
    }
}
