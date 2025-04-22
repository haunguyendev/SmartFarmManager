using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.BusinessModels.FarmSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.ExternalClient
{
    public class ExternalFarmApiClient
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalFarmApiClient> _logger;

        public ExternalFarmApiClient( HttpClient httpClient,IUnitOfWork unitOfWork,ILogger<ExternalFarmApiClient> logger)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<FarmSyncModel?> GetFarmDataAsync(Guid farmId)
        {
            var apiKey = await _unitOfWork.WhiteListDomains.FindByCondition(x => x.Domain == "api-trangtrai.nongdanonline.vn").Select(x => x.ApiKey).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(apiKey))
                throw new UnauthorizedAccessException("Không tìm thấy API key hợp lệ cho domain yêu cầu.");
            var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://api-trangtrai.nongdanonline.vn/api/farms/for-webhook/{farmId}");
            request.Headers.Add("x-api-key", apiKey);

            var response = await _httpClient.SendAsync(request);
            
            _logger.LogInformation($"Response status code: {response.StatusCode}");
            _logger.LogInformation($"Response content: {await response.Content.ReadAsStringAsync()}");
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FarmSyncModel>(content);
        }
    }

}
