using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.DeadPoultryLog
{
    public class DeadPoultryLogResponseModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }

        // Thông tin FarmingBatch
        public Guid FarmingBatchId { get; set; }
        public string FarmingBatchName { get; set; }

        // Thông tin Cage
        public Guid CageId { get; set; }
        public string CageName { get; set; }
    }
}
