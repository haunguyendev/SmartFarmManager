﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.BusinessModels.Farm
{
    public class FarmModel
    {
        public Guid Id { get; set; }
        public Guid? ExternalId { get;set; }
        public string Name { get; set; }
        public string FarmCode { get; set; }
        public string Address { get; set; }
        public double Area { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

}
