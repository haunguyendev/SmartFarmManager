using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.FarmingBatch
{
    public class GroupFarmingBatchModel
    {
        public string FarmingBatchName { get; set; }
        public DateTime? DateStart { get; set; }

        public List<FarmingBatchModel> farmingBatchModels { get; set; }
    }
}
