using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.DataAccessObject.Models
{
    public class DeadPoultryLog : EntityBase
    {
        public Guid FarmingBatchId { get; set; }
        public DateTime Date { get; set; } 
        public int Quantity { get; set; } 
        public string? Note { get; set; } 
        public virtual FarmingBatch FarmingBatch { get; set; }
    }
}
