using Microsoft.EntityFrameworkCore;
using SmartFarmManager.DataAccessObject.Models;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.BusinessModels.CostingReport;
using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.Helpers;
using SmartFarmManager.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Services
{
    public class CostingService : ICostingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CostingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<CostingReportItemModel>> GetCostingReportsAsync(CostingReportFilterModel filter)
        {
            var query = _unitOfWork.CostingReports
                .FindAll(false)
                .Include(r => r.Farm)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.KeySearch))
            {
                query = query.Where(r =>
                    r.Farm.Name.Contains(filter.KeySearch) ||
                    r.CostType.Contains(filter.KeySearch));
            }

            if (filter.FarmId.HasValue)
            {
                query = query.Where(r => r.FarmId == filter.FarmId.Value);
            }

            if (!string.IsNullOrEmpty(filter.CostType))
            {
                query = query.Where(r => r.CostType == filter.CostType);
            }

            if (filter.ReportMonth.HasValue)
            {
                query = query.Where(r => r.ReportMonth == filter.ReportMonth.Value);
            }

            if (filter.ReportYear.HasValue)
            {
                query = query.Where(r => r.ReportYear == filter.ReportYear.Value);
            }

            query = query.OrderByDescending(r => r.GeneratedAt);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(r => new CostingReportItemModel
                {
                    Id = r.Id,
                    FarmId = r.FarmId,
                    FarmName = r.Farm.Name,
                    CostType = r.CostType,
                    TotalQuantity = r.TotalQuantity,
                    TotalCost = r.TotalCost,
                    ReportMonth = r.ReportMonth,
                    ReportYear = r.ReportYear,
                    GeneratedAt = r.GeneratedAt
                })
                .ToListAsync();

            var result = new PaginatedList<CostingReportItemModel>(items, totalItems, filter.PageNumber, filter.PageSize);

            return new PagedResult<CostingReportItemModel>
            {
                Items = result.Items,
                TotalItems = result.TotalCount,
                PageSize = result.PageSize,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage
            };
        }


        public async System.Threading.Tasks.Task CalculateAndStoreDailyCostAsync()
        {
            var today = DateTimeUtils.GetServerTimeInVietnamTime().Date;
            var reportMonth = today.Month;
            var reportYear = today.Year;

            var farms = await _unitOfWork.Farms.FindAll().ToListAsync();

            foreach (var farm in farms)
            {
                var existingReport = await _unitOfWork.CostingReports
                    .FindByCondition(r => r.FarmId == farm.Id && r.ReportMonth == reportMonth && r.ReportYear == reportYear)
                    .ToListAsync();

                var costData = await GetDailyCostsForFarm(farm.Id, today);

                foreach (var cost in costData)
                {
                    var existingCostEntry = existingReport.FirstOrDefault(r => r.CostType == cost.CostType);

                    if (existingCostEntry != null)
                    {
                        // Cập nhật báo cáo
                        existingCostEntry.TotalQuantity += cost.TotalQuantity;
                        existingCostEntry.TotalCost += cost.TotalCost;
                        existingCostEntry.GeneratedAt = today;

                        await _unitOfWork.CostingReports.UpdateAsync(existingCostEntry);
                    }
                    else
                    {
                        // Tạo báo cáo mới
                        var newReport = new CostingReport
                        {
                            Id = Guid.NewGuid(),
                            FarmId = farm.Id,
                            ReportMonth = reportMonth,
                            ReportYear = reportYear,
                            CostType = cost.CostType,
                            TotalQuantity = cost.TotalQuantity,
                            TotalCost = cost.TotalCost,
                            GeneratedAt = today
                        };

                        await _unitOfWork.CostingReports.CreateAsync(newReport);
                    }
                }
            }

            await _unitOfWork.CommitAsync();
        }

        private async Task<List<CostingReport>> GetDailyCostsForFarm(Guid farmId, DateTime date)
        {
            var costReports = new List<CostingReport>();

            var electricityLogs = await _unitOfWork.ElectricityLogs
                .FindByCondition(e => e.FarmId == farmId && e.CreatedDate.Date == date)
                .ToListAsync();

            var electricityUsage = electricityLogs.Sum(e => e.TotalConsumption);
            var electricityPrice = await _unitOfWork.MasterData
                .FindByCondition(m => m.FarmId == farmId && m.CostType == "Điện")
                .Select(m => m.UnitPrice)
                .FirstOrDefaultAsync();

            costReports.Add(new CostingReport
            {
                CostType = "Điện",
                TotalQuantity = (decimal)electricityUsage,
                TotalCost = (decimal)electricityUsage * electricityPrice
            });

            // 2️⃣ Chi phí nước
            var waterLogs = await _unitOfWork.WaterLogs
                .FindByCondition(w => w.FarmId == farmId && w.CreatedDate.Date == date)
                .ToListAsync();

            var waterUsage = waterLogs.Sum(w => w.TotalConsumption);
            var waterPrice = await _unitOfWork.MasterData
                .FindByCondition(m => m.FarmId == farmId && m.CostType == "Nước")
                .Select(m => m.UnitPrice)
                .FirstOrDefaultAsync();

            costReports.Add(new CostingReport
            {
                CostType = "Nước",
                TotalQuantity = (decimal)waterUsage,
                TotalCost = (decimal)waterUsage * waterPrice
            });

            var foodLogs = await _unitOfWork.DailyFoodUsageLogs
                .FindByCondition(f => f.Stage.FarmingBatch.FarmId == farmId && f.LogTime.Value.Date == date)
                .ToListAsync();

            var totalFoodCost = foodLogs.Sum(f => f.ActualWeight.Value * (decimal)f.UnitPrice);

            costReports.Add(new CostingReport
            {
                CostType = "Thức ăn",
                TotalQuantity = foodLogs.Sum(f => f.ActualWeight ?? 0),
                TotalCost = totalFoodCost
            });

            // 4️⃣ Chi phí vaccine
            var vaccineLogs = await _unitOfWork.VaccineScheduleLogs
                .FindByCondition(v => v.Schedule.Stage.FarmingBatch.FarmId == farmId && v.Date.Value == DateOnly.FromDateTime(date))
                .ToListAsync();

            var totalVaccineCost = vaccineLogs.Sum(v => v.Schedule.ToltalPrice);

            costReports.Add(new CostingReport
            {
                CostType = "Vaccine",
                TotalQuantity = vaccineLogs.Count(),
                TotalCost = totalVaccineCost.Value
            });

            var prescriptionLogs = await _unitOfWork.Prescription
                .FindByCondition(p => p.MedicalSymtom.FarmingBatch.FarmId == farmId && p.PrescribedDate.Value.Date == date)
                .ToListAsync();

            var totalMedicineCost = prescriptionLogs.Sum(p => p.Price ?? 0);

            costReports.Add(new CostingReport
            {
                CostType = "Thuốc",
                TotalQuantity = prescriptionLogs.Count,
                TotalCost = totalMedicineCost
            });

           
            return costReports;
        }
    }

}
