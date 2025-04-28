using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.CostingReport
{
    public class CostingReportGroupFilterModel
    {
        public int? Year { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
