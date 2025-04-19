using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.MasterData
{
    public class MasterDataItemModel
    {
        public Guid Id { get; set; }
        public string CostType { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid FarmId { get; set; }
        public string FarmName { get; set; }
    }
}
