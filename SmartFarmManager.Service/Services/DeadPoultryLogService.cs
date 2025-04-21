using SmartFarmManager.Repository;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.BusinessModels.DeadPoultryLog;
using SmartFarmManager.Service.BusinessModels;
using SmartFarmManager.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartFarmManager.Service.Services
{
    public class DeadPoultryLogService : IDeadPoultryLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeadPoultryLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // DeadPoultryLogService.cs
        public async Task<PagedResult<DeadPoultryLogResponseModel>> GetDeadPoultryLogsAsync( string? cageName, string? farmingBatchName, string? note, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            var query = _unitOfWork.DeadPoultryLogs.FindAll()
                .Include(d => d.FarmingBatch)
                    .ThenInclude(fb => fb.Cage)
                .AsQueryable();

            // Áp dụng bộ lọc
            if (!string.IsNullOrEmpty(cageName))
            {
                query = query.Where(d =>
                    d.FarmingBatch.Cage.Name.Contains(cageName));
            }

            if (!string.IsNullOrEmpty(farmingBatchName))
            {
                query = query.Where(d =>
                    d.FarmingBatch.Name.Contains(farmingBatchName));
            }

            if (!string.IsNullOrEmpty(note))
            {
                query = query.Where(d =>
                    d.Note.Contains(note));
            }

            // Thêm bộ lọc ngày
            if (startDate.HasValue)
                query = query.Where(d => d.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(d => d.Date <= endDate.Value);

            // Phân trang
            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(d => d.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DeadPoultryLogResponseModel
                {
                    Id = d.Id,
                    Date = d.Date,
                    Quantity = d.Quantity,
                    Note = d.Note,
                    FarmingBatchId = d.FarmingBatch.Id,
                    FarmingBatchName = d.FarmingBatch.Name,
                    CageId = d.FarmingBatch.Cage.Id,
                    CageName = d.FarmingBatch.Cage.Name
                })
                .ToListAsync();

            return new PagedResult<DeadPoultryLogResponseModel>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                HasNextPage = pageNumber < (int)Math.Ceiling(totalItems / (double)pageSize),
                HasPreviousPage = pageNumber > 1
            };
        }

    }
}
