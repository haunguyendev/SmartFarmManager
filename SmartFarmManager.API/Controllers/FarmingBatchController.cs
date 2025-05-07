using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.API.Common;
using SmartFarmManager.API.Payloads.Requests.FarmingBatch;
using SmartFarmManager.Service.BusinessModels.Task;
using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.Interfaces;
using Sprache;
using SmartFarmManager.Service.BusinessModels.FarmingBatch;
using SmartFarmManager.Service.BusinessModels.Cages;
using Microsoft.AspNetCore.Authorization;

namespace SmartFarmManager.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class FarmingBatchController : ControllerBase
    {
        private readonly IFarmingBatchService _farmingBatchService;

        public FarmingBatchController(IFarmingBatchService farmingBatchService)
        {
            _farmingBatchService = farmingBatchService;
        }


        [HttpPost()]
        [Authorize(Roles = "Admin Farm")]
        public async Task<IActionResult> CreateFarmingBatch([FromBody] CreateFarmingBatchRequest request)
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
                var model = request.MapToModel();
                var result = await _farmingBatchService.CreateFarmingBatchAsync(model);

                if (!result)
                {
                    return BadRequest(ApiResult<string>.Fail("Tạo vụ nuôi thất bại. Vui lòng thử lại."));
                }

                return Ok(ApiResult<string>.Succeed("Tạo vụ nuôi thành công!"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResult<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail(ex.Message));
            }
        }

        [HttpPost("create-multi-cage")]
        public async Task<IActionResult> CreateFarmingBatchMultiCage([FromBody] CreateFarmingBatchMultiCageRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Errors = errors });
            }

            try
            {
                var result = await _farmingBatchService.CreateFarmingBatchMultiCageAsync(request.MapToModel());

                if (result)
                {
                    return Ok(new { Message = "Tạo vụ nuôi cho nhiều chuồng thành công." });
                }

                return BadRequest(new { Message = "Tạo vụ nuôi thất bại." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin Farm")]
        public async Task<IActionResult> UpdateFarmingBatchStatus(Guid id, [FromBody] UpdateFarmingBatchStatusRequest request)
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
                var result = await _farmingBatchService.UpdateFarmingBatchStatusAsync(id, request.NewStatus);

                if (!result)
                {
                    return BadRequest(ApiResult<string>.Fail("Cập nhật trạng thái vụ nuôi thất bại. Vui lòng thử lại."));
                }

                return Ok(ApiResult<string>.Succeed("Cập nhật trạng thái vụ nuôi thành công!"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResult<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail(ex.Message));
            }
        }
        [HttpPost("update-status-today")]
        public async Task<IActionResult> UpdateFarmingBatchStatusToday()
        {
            try
            {
                // Gọi hàm kiểm tra và cập nhật trạng thái vụ nuôi có ngày bắt đầu là hôm nay
                await _farmingBatchService.RunUpdateFarmingBatchesStatusAsync();

                return Ok(new { Message = "Cập nhật trạng thái vụ nuôi cho ngày hôm nay thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin Farm")]
        public async Task<IActionResult> GetFarmingBatches([FromQuery] FarmingBatchFilterPagingRequest request)
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
                var response = await _farmingBatchService.GetFarmingBatchesAsync(request.KeySearch, request.FarmId, request.CageName, request.Name, request.Name, request.StartDateFrom, request.StartDateTo, request.PageNumber, request.PageSize, request.CageId, request.isCancel);

                return Ok(ApiResult<PagedResult<FarmingBatchModel>>.Succeed(response));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResult<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail("Đã xảy ra lỗi không mong muốn. Vui lòng liên hệ bộ phận hỗ trợ."));
            }
        }

        [HttpGet("cage/{cageId:guid}")]
        public async Task<IActionResult> GetActiveFarmingBatchByCageId(Guid cageId)
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
                var farmingBatch = await _farmingBatchService.GetActiveFarmingBatchByCageIdAsync(cageId);

                if (farmingBatch == null)
                    return NotFound(ApiResult<string>.Fail("Không tìm thấy vụ nuôi đang hoạt động cho chuồng này"));

                return Ok(ApiResult<FarmingBatchModel>.Succeed(farmingBatch));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResult<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail("Đã xảy ra lỗi không mong muốn. Vui lòng liên hệ bộ phận hỗ trợ."));
            }
        }
        [HttpGet("cage/{cageId:guid}/{dueDateTask:Datetime}")]
        [Authorize(Roles = "Staff Farm")]
        public async Task<IActionResult> GetFarmingBatchByCageIdAndueDateTask(Guid cageId, DateTime dueDateTask)
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
                var farmingBatch = await _farmingBatchService.GetFarmingBatchByCageIdAndueDateTaskAsync(cageId, dueDateTask);

                if (farmingBatch == null)
                    return NotFound(ApiResult<string>.Fail("Không tìm thấy vụ nuôi đang hoạt động cho chuồng này"));

                return Ok(ApiResult<FarmingBatchModel>.Succeed(farmingBatch));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResult<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail("Đã xảy ra lỗi không mong muốn. Vui lòng liên hệ bộ phận hỗ trợ."));
            }
        }
        [HttpGet("active-batches-by-user")]
        public async Task<IActionResult> GetActiveBatchesByUser([FromQuery] Guid userId)
        {
            var activeBatches = await _farmingBatchService.GetActiveFarmingBatchesByUserAsync(userId);
            return Ok(ApiResult<List<FarmingBatchModel>>.Succeed(activeBatches));
        }

        [HttpGet("{farmingBatchId}/report")]
        public async Task<IActionResult> GetFarmingBatchReport(Guid farmingBatchId)
        {
            var report = await _farmingBatchService.GetFarmingBatchReportAsync(farmingBatchId);
            if (report == null)
                return NotFound(ApiResult<object>.Fail("Không tìm thấy vụ nuôi."));

            return Ok(ApiResult<FarmingBatchReportResponse>.Succeed(report));
        }

        /// 📌 **API: Báo cáo chi tiết Farming Batch**
        [HttpGet("{farmingBatchId}/detailed-report")]
        [Authorize(Roles = "Admin Farm, Customer")]
        public async Task<IActionResult> GetDetailedFarmingBatchReport(Guid farmingBatchId)
        {
            var report = await _farmingBatchService.GetDetailedFarmingBatchReportAsync(farmingBatchId);
            if (report == null)
                return NotFound(ApiResult<object>.Fail("Không tìm thấy vụ nuôi."));

            return Ok(ApiResult<DetailedFarmingBatchReportResponse>.Succeed(report));
        }

        [HttpGet("{cageId}/current-farming-stage")]
        public async Task<IActionResult> GetCurrentFarmingStage(Guid cageId)
        {
            try
            {
                var result = await _farmingBatchService.GetCurrentFarmingStageWithCageAsync(cageId);

                if (result == null)
                    return NotFound(ApiResult<object>.Fail("Không tìm thấy vụ nuôi đang hoạt động cho chuồng này."));

                return Ok(ApiResult<CageFarmingStageModel>.Succeed(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }


        [HttpPost("check-upcoming-farming-batches")]
        public async Task<IActionResult> CheckAndNotifyAdminForUpcomingFarmingBatches()
        {
            try
            {
                await _farmingBatchService.CheckAndNotifyAdminForUpcomingFarmingBatchesAsync();

                return Ok(new { Message = "Đã kiểm tra và thông báo cho quản trị viên về các vụ nuôi sắp tới." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }
        [HttpPost("check-ending-farming-batches")]
        public async Task<IActionResult> CheckAndNotifyAdminForEndingFarmingBatches()
        {
            try
            {
                await _farmingBatchService.CheckAndNotifyAdminForEndingFarmingBatchesAsync();

                return Ok(new { Message = "Đã kiểm tra và thông báo cho quản trị viên về các vụ nuôi sắp kết thúc." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

        [HttpPut("{farmingBatchId}/update-start-date")]
        public async Task<IActionResult> UpdateStartDate(Guid farmingBatchId, DateTime newStartDate)
        {
            try
            {
                var result = await _farmingBatchService.UpdateStartDateAsync(farmingBatchId, newStartDate);
                return Ok(new { Message = "Cập nhật ngày bắt đầu thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{farmingBatchId}")]
        [Authorize(Roles = "Customer, Staff Farm")]
        public async Task<IActionResult> GetFarmingBatchDetail(Guid farmingBatchId)
        {
            try
            {
                var result = await _farmingBatchService.GetFarmingBatchDetailAsync(farmingBatchId);
                return Ok(ApiResult<FarmingBatchDetailModel>.Succeed(result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResult<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail($"Đã xảy ra lỗi: {ex.Message}"));
            }
        }

        [HttpGet("customer/{userId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetFarmingBatchesForCustomer(Guid userId)
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
                var response = await _farmingBatchService.GetGroupedFarmingBatchesByUser(userId);

                return Ok(ApiResult<List<GroupFarmingBatchModel>>.Succeed(response));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResult<string>.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResult<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail("Đã xảy ra lỗi không mong muốn. Vui lòng liên hệ bộ phận hỗ trợ."));
            }
        }

        [HttpPost("{farmingBatchId}/growth-stages/{growthStageId}/dead-animals")]
        [Authorize(Roles = "Staff Farm")]
        public async Task<IActionResult> UpdateDeadAnimals(
        Guid farmingBatchId,
        Guid growthStageId,
        [FromBody] UpdateDeadAnimalRequest request)
        {
            try
            {
                var result = await _farmingBatchService.UpdateDeadAnimalsAsync(
                    farmingBatchId,
                    growthStageId,
                    request.DeadAnimal,
                    request.Note);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ", Details = ex.Message });
            }
        }
    }
}