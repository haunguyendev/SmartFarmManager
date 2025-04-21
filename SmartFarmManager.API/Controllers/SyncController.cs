using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.API.Common;
using SmartFarmManager.Service.Interfaces;

namespace SmartFarmManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;
        public SyncController(ISyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost("farm/{farmId}")]
        public async Task<IActionResult> SyncFarm(Guid farmId)
        {

            try 
            {
                await _syncService.SyncFarmFromExternalAsync(farmId);
                return Ok(ApiResult<string>.Succeed("Đồng bộ thành công!"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResult<string>.Fail("Bạn không có quyền truy cập vào hệ thống"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResult<string>.Fail(ex.Message));
            }
        }
    }
}
