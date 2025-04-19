using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.Service.Interfaces;

namespace SmartFarmManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return Ok("✅ Đồng bộ thành công");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized($"❌ Không hợp lệ: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Lỗi khi đồng bộ: {ex.Message}");
            }
        }
    }
}
