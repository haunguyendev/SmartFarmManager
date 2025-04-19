using Microsoft.EntityFrameworkCore;
using SmartFarmManager.Repository.Interfaces;
using SmartFarmManager.Service.BusinessModels.MasterData;
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
    public class MasterDataService : IMasterDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MasterDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
       }
    
    public async Task<PagedResult<MasterDataItemModel>> GetMasterDataAsync(MasterDataFilterModel filter)
        {
            var query = _unitOfWork.MasterData.FindAll(false).AsQueryable();

            if (!string.IsNullOrEmpty(filter.KeySearch))
            {
                query = query.Where(md =>
                    md.CostType.Contains(filter.KeySearch) ||
                    md.Unit.Contains(filter.KeySearch) ||
                    md.UnitPrice.ToString().Contains(filter.KeySearch));
            }

            if (!string.IsNullOrEmpty(filter.CostType))
            {
                query = query.Where(md => md.CostType.Contains(filter.CostType));
            }

            if (filter.FarmId.HasValue)
            {
                query = query.Where(md => md.FarmId == filter.FarmId.Value);
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(md => new MasterDataItemModel
                {
                    Id = md.Id,
                    CostType = md.CostType,
                    Unit = md.Unit,
                    UnitPrice = md.UnitPrice,
                    FarmId = md.FarmId,
                    FarmName = md.Farm.Name
                })
                .ToListAsync();

            var result = new PaginatedList<MasterDataItemModel>(items, totalItems, filter.PageNumber, filter.PageSize);

            return new PagedResult<MasterDataItemModel>
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
    }
}
