﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.GrowthStageTemplate
{
    public class UpdateGrowthStageTemplateModel
    {
        public string? StageName { get; set; }
        public decimal? WeightAnimal { get; set; }
        public int? AgeStart { get; set; }
        public int? AgeEnd { get; set; }
        public string? Notes { get; set; }
        public Guid? SaleTypeId { get; set; }
    }
}
