﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.DailyFoodUsageLog
{
    public class FoodUsageDetail
    {
        public string FoodType { get; set; }
        public decimal TotalWeightUsed { get; set; }
        
        public double UnitPrice { get; set; }
        public decimal TotalCost {  get; set; }
        public string GrowStageNames { get; set; }
    }
}
