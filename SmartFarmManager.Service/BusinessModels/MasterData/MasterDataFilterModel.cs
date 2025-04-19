using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.MasterData
{
    public class MasterDataFilterModel
    {
        public string? KeySearch { get; set; }
        public string? CostType { get; set; }
        public Guid? FarmId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
