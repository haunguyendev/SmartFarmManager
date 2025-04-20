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

        [HttpPost("sync/farm/{farmCode}")]
        public async Task<IActionResult> SyncFarm(string farmCode)
        {

            try 
            {
                await _syncService.SyncFarmFromExternalAsync(farmCode);
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
