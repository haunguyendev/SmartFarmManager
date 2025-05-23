﻿using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.BusinessModels.Cages;
using SmartFarmManager.Service.BusinessModels.Prescription;
using SmartFarmManager.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Interfaces
{
    public interface ICageService
    {
        Task<PagedResult<CageResponseModel>> GetCagesAsync(CageFilterModel request);
        Task<CageDetailModel> GetCageByIdAsync(Guid cageId);
        Task<List<CageResponseModel>> GetUserCagesAsync(Guid userId);
        Task<Guid> CreateCageAsync(CageModel model);
        Task<IEnumerable<CageModel>> GetAllCagesAsync(string? search);
        Task<bool> UpdateCageAsync(Guid id, CageModel model);
        Task<bool> DeleteCageAsync(Guid id);
        Task<CageIsolationResponseModel> GetPrescriptionsWithTasksAsync();

        Task<CageIsolateModel?> GetCageIsolatePrescriptionsWithDetailsAsync();
    }
}
