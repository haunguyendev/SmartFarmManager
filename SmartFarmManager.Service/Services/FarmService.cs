using Microsoft.EntityFrameworkCore;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.BusinessModels.CostingReport;
using SmartFarmManager.Service.BusinessModels.Farm;
using SmartFarmManager.Service.Helpers;
using SmartFarmManager.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Services
{
    public class FarmService : IFarmService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FarmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateFarmAsync(FarmModel model)
        {
            var farm = new Farm
            {
                Name = model.Name,
                Address = model.Address,
                Area = model.Area,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CreatedDate = DateTimeUtils.GetServerTimeInVietnamTime()
            };

            var id = await _unitOfWork.Farms.CreateAsync(farm);
            await _unitOfWork.CommitAsync();
            return id;
        }

        public async Task<FarmModel> GetFarmByIdAsync(Guid id)
        {
            var farm = await _unitOfWork.Farms.GetByIdAsync(id);
            if (farm == null) return null;

            return new FarmModel
            {
                Id = farm.Id,
                ExternalId =(Guid)farm.ExternalId,
                Name = farm.Name,
                Address = farm.Address,
                Area = farm.Area,
                PhoneNumber = farm.PhoneNumber,
                Email = farm.Email
            };
        }

        public async Task<IEnumerable<FarmModel>> GetAllFarmsAsync(string? search)
        {
            var farms = await _unitOfWork.Farms.FindAllAsync(f => string.IsNullOrEmpty(search) || f.Name.Contains(search));

            return farms.Select(f => new FarmModel
            {
                Id = f.Id,
                ExternalId = (Guid)f.ExternalId,
                Name = f.Name,
                FarmCode = f.FarmCode,
                Address = f.Address,
                Area = f.Area,
                PhoneNumber = f.PhoneNumber,
                Email = f.Email
            });
        }

        public async Task<bool> UpdateFarmAsync(Guid id, FarmUpdateModel model)
        {
            var farm = await _unitOfWork.Farms.FindByCondition(f => f.Id == id).FirstOrDefaultAsync();

            if (farm == null)
            {
                throw new KeyNotFoundException($"Nông trại với {id} không tìm thấy.");
            }

            var farmWithSameCode = await _unitOfWork.Farms
                .FindByCondition(f => f.FarmCode == model.FarmCode && f.Id != id)
                .FirstOrDefaultAsync();

            if (farmWithSameCode != null)
            {
                throw new ArgumentException($"Nông trại với '{model.FarmCode}' đã tồn tại");
            }
            farm.ExternalId = model.ExternalId??farm.ExternalId;
            farm.FarmCode = model.FarmCode ?? farm.FarmCode;
            farm.Name = model.Name ?? farm.Name;
            farm.Address = model.Address ?? farm.Address;
            farm.PhoneNumber = model.PhoneNumber ?? farm.PhoneNumber;
            farm.Email = model.Email ?? farm.Email;
            farm.Area = model.Area ?? farm.Area;
            farm.Macaddress = model.Macaddress ?? farm.Macaddress;
            farm.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Farms.UpdateAsync(farm);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteFarmAsync(Guid id)
        {
            var farm = await _unitOfWork.Farms.GetByIdAsync(id);
            if (farm == null) return false;

            farm.IsDeleted = true;
            farm.DeletedDate = DateTimeUtils.GetServerTimeInVietnamTime();

            await _unitOfWork.Farms.UpdateAsync(farm);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<CostingReportModel>> GetCostingReportsByFarmAsync(Guid farmId, int? month, int? year)
        {
            var query = _unitOfWork.CostingReports
                .FindByCondition(c => c.FarmId == farmId);

            if (month.HasValue)
                query = query.Where(c => c.ReportMonth == month.Value);

            if (year.HasValue)
                query = query.Where(c => c.ReportYear == year.Value);

            var reports = await query.ToListAsync();

            return reports.Select(c => new CostingReportModel
            {
                Id = c.Id,
                ReportMonth = c.ReportMonth,
                ReportYear = c.ReportYear,
                CostType = c.CostType,
                TotalQuantity = c.TotalQuantity,
                TotalCost = c.TotalCost,
                GeneratedAt = c.GeneratedAt
            });
        }
    }

}
