using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.API.Common;
using SmartFarmManager.API.Payloads.Requests.CostingReport;
using SmartFarmManager.Service.BusinessModels.CostingReport;
using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.Interfaces;

namespace SmartFarmManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostingReportController : ControllerBase
    {
        private readonly ICostingService _costingReportService;
        public CostingReportController(ICostingService costingService)
        {
            _costingReportService = costingService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCostingReports([FromQuery] CostingReportFilterRequest filterRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
                return BadRequest(ApiResult<Dictionary<string, string[]>>.Error(new Dictionary<string, string[]>
        {
            { "Errors", errors.ToArray() }
        }));
            }

            try
            {
                var filterModel = new CostingReportFilterModel
                {
                    KeySearch = filterRequest.KeySearch,
                    FarmId = filterRequest.FarmId,
                    CostType = filterRequest.CostType,
                    ReportMonth = filterRequest.ReportMonth,
                    ReportYear = filterRequest.ReportYear,
                    PageNumber = filterRequest.PageNumber,
                    PageSize = filterRequest.PageSize
                };

                var result = await _costingReportService.GetCostingReportsAsync(filterModel);
                return Ok(ApiResult<PagedResult<CostingReportItemModel>>.Succeed(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail(ex.Message));
            }
        }
    }
}
