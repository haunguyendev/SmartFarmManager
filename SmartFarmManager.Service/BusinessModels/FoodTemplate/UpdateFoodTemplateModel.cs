﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.FoodTemplate
{
    public class UpdateFoodTemplateModel
    {
        public Guid Id { get; set; }
        public string? FoodType { get; set; }
        public decimal? WeightBasedOnBodyMass { get; set; }
    }
}
