using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.Task
{
    public class CreateSaleTaskModel
    {
        public Guid FarmingBatchId { get; set; }
        public DateTime DueDate { get; set; }
        public string? Notes { get; set; }
        public Guid CreatedByUserId { get; set; }
    }
}
