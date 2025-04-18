using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFarmManager.API.Payloads.Requests.Webhook;
using SmartFarmManager.Service.Interfaces;
using System.Text.Json;

namespace SmartFarmManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IWebhookService _webhookService;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(IWebhookService webhookService, ILogger<WebhookController> logger)
        {
            _webhookService = webhookService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveData([FromBody] WebhookRequest webhookRequest)
        {
            try
            {
                // Kiểm tra `x-api-key`
                if (!Request.Headers.ContainsKey("x-api-key") || string.IsNullOrEmpty(Request.Headers["x-api-key"].ToString()))
                {
                    _logger.LogWarning("❌ Thiếu x-api-key trong header.");
                    return Unauthorized("Thiếu x-api-key.");
                }

                var apiKey = Request.Headers["x-api-key"].ToString();
                var domain = Request.Headers["Origin"].ToString() ?? Request.Headers["Referer"].ToString();
                if (string.IsNullOrEmpty(domain))
                {
                    domain = Request.Headers["x-origin-domain"].ToString();
                }
                if (string.IsNullOrEmpty(domain))
                {
                    domain = Request.Host.Host;
                }

                // Validate `x-api-key` và `domain` qua service
                var isValid = await _webhookService.ValidateApiKeyAsync(apiKey, domain);
                if (!isValid)
                {
                    return Unauthorized("API Key hoặc Domain không hợp lệ.");
                }

                var dataObject = webhookRequest.Data;
                string jsonData = JsonSerializer.Serialize(dataObject);

                _logger.LogInformation("📡 JSON Payload: {JsonPayload}", jsonData);
                _logger.LogInformation("✅ Nhận dữ liệu webhook từ domain {Domain}", domain);
                _logger.LogInformation("🔹 Datatype: {Datatype}", webhookRequest.Datatype);
                try
                {

                    await _webhookService.HandleWebhookDataAsync(webhookRequest.Datatype, jsonData);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "❌ Đã xảy ra lỗi khi xử lý webhook: {Message}", ex.Message);
                    return BadRequest($"Lỗi xử lý webhook: {ex.Message}");
                }
                return Ok("✅ Dữ liệu đã được nhận thành công!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Đã xảy ra lỗi khi xử lý webhook: {Message}", ex.Message);
                return BadRequest($"Lỗi xử lý webhook: {ex.Message}");
            }
        }

    }

}
