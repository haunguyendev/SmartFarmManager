using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.AnimalSale
{
    public class AnimalMeatSaleModel
    {
        public Guid Id { get; set; }
        public Guid FarmingBatchId { get; set; }
        public DateTime? SaleDate { get; set; }
        public double Total { get; set; }
        public decimal? UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Guid StaffId { get; set; }
        public Guid SaleTypeId { get; set; }
        public decimal? Weight { get; set; }

        public string SaleTypeName { get; set; }
        public decimal? ExpectTotalWeight { get; set; }
    }

}
