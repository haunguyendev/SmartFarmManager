using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.CostingReport
{
    public class CostingReportGroupModel
    {
        public int ReportMonth { get; set; }
        public int ReportYear { get; set; }
        public CostDetail? Electricity { get; set; }
        public CostDetail? Water { get; set; }
        public CostDetail? Food { get; set; }
        public CostDetail? Vaccine { get; set; }
        public CostDetail? Medicine { get; set; }
    }

    public class CostDetail
    {
        public decimal TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }
    }
}
