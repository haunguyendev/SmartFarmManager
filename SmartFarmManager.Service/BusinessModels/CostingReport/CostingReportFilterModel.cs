using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.CostingReport
{
    public class CostingReportFilterModel
    {
        public string? KeySearch { get; set; }
        public Guid? FarmId { get; set; }
        public string? CostType { get; set; }
        public int? ReportMonth { get; set; }
        public int? ReportYear { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
