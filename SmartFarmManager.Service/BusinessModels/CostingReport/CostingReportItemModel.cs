using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.CostingReport
{
    public class CostingReportItemModel
    {
        public Guid Id { get; set; }
        public Guid FarmId { get; set; }
        public string FarmName { get; set; }
        public string CostType { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }
        public int ReportMonth { get; set; }
        public int ReportYear { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
