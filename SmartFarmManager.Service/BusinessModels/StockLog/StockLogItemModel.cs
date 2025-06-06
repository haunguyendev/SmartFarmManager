﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.StockLog
{
    public class StockLogItemModel
    {
        public Guid Id { get; set; }
        public string FoodType { get; set; }
        public decimal Quantity { get; set; }
        public decimal CostPerKg { get; set; }
        public DateOnly DateAdded { get; set; }
    }
}
